// -----------------------------------------------------------------------
// <copyright file="TelegramBot.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DjayEnglish.Core;
    using DjayEnglish.Core.ObjectModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Telegram.Bot;
    using Telegram.Bot.Exceptions;
    using Telegram.Bot.Extensions.Polling;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.InputFiles;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Telegram bot.
    /// </summary>
    public class TelegramBot
    {
        private readonly ILogger<TelegramBot> logger;
        private readonly TelegramBotClient bot;
        private readonly IAudioProvider audioProvider;
        private readonly QuizeManager quizeManager;
        private readonly Dictionary<long, Quize> chatQuize = new Dictionary<long, Quize>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramBot"/> class.
        /// </summary>
        /// <param name="logger">Logger to use in current class.</param>
        /// <param name="configuration">Current App configuration.</param>
        /// <param name="audioProvider">Audio file provider.</param>
        /// <param name="quizeManager">Quize manager to use.</param>
        public TelegramBot(
            ILogger<TelegramBot> logger,
            IConfiguration configuration,
            IAudioProvider audioProvider,
            QuizeManager quizeManager)
        {
            var accessToken = configuration["TelegramBot:AccessToken"];
            this.bot = new TelegramBotClient(accessToken);
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.audioProvider = audioProvider ?? throw new ArgumentNullException(nameof(audioProvider));
            this.quizeManager = quizeManager ?? throw new ArgumentNullException(nameof(quizeManager));
        }

        /// <summary>
        /// Initialize telegram bot.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task InitializeAsync()
        {
            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            this.bot.StartReceiving(
                new DefaultUpdateHandler(this.HandleUpdateAsync, this.HandleErrorAsync));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop telegram bot.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task StopAsync()
        {
            this.bot.StopReceiving();
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => this.BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage => this.BotOnMessageReceived(update.Message),
                UpdateType.CallbackQuery => this.BotOnCallbackQueryReceived(update.CallbackQuery),
                _ => this.UnknownUpdateHandlerAsync(update),
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await this.HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return;
            }

            var chatId = message.Chat.Id;
            var action = message.Text.Split(' ').First();
            switch (action)
            {
                case "/quize": await this.SendQuizeAsync(chatId).ConfigureAwait(false);
                    return;
            }

            if (this.IsQuizeAnswer(chatId, action))
            {
                await this.RegisterUserAnswerAsync(chatId, action).ConfigureAwait(false);
                return;
            }

            await this.Usage(message).ConfigureAwait(false);
        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            await this.bot.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}");

            await this.bot.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}");
        }

        private async Task Usage(Message message)
        {
            var usage = @"
Usage:
/quize    - start quize";
            await this.bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove());
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            this.logger.LogError($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString(),
            };

            this.logger.LogError(errorMessage);
            return Task.CompletedTask;
        }

        private bool IsQuizeAnswer(long chatId, string command)
        {
            var quizeIsRuning = this.chatQuize.TryGetValue(chatId, out var quize);
            if (!quizeIsRuning || quize == null)
            {
                return false;
            }

            var showedAnswerOptions = quize.GetShowedQuizeAnswerOptions();
            return showedAnswerOptions.Contains(command);
        }

        private async Task RegisterUserAnswerAsync(long chatId, string userAnswer)
        {
            var result = this.quizeManager.RegisterUserAnswer(chatId, userAnswer);
            if (result.IsRight)
            {
                await this.bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: "You are right!");
            }
            else
            {
                await this.bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: "This is not the right answer!");
            }
        }

        private async Task SendQuizeAsync(long chatId)
        {
            var quizeIsRuning = this.chatQuize.TryGetValue(chatId, out var quize);
            if (!quizeIsRuning || quize == null)
            {
                quize = this.quizeManager.StartQuize(chatId);
            }

            var question = quize.GetQuizeText();
            var examples = quize.GetExamplesText();
            var answerOptions = quize.GetAnswerOptionsText();
            var showedAnswerOptions = quize.GetShowedQuizeAnswerOptions();

            await this.SendAudioFileAsync(chatId, question, "Question").ConfigureAwait(false);
            await this.SendAudioFileAsync(chatId, examples, "Examples").ConfigureAwait(false);
            await this.SendAudioFileAsync(chatId, answerOptions, "Answer options").ConfigureAwait(false);

            await this.SendReplyKeyboard(chatId, showedAnswerOptions).ConfigureAwait(false);
        }

        private async Task SendAudioFileAsync(long chatId, string text, string caption)
        {
            await this.bot.SendChatActionAsync(chatId, ChatAction.UploadAudio);
            using var audioFile = await this.audioProvider.GetAudio(text).ConfigureAwait(false);
            var inputOnlineFile = new InputOnlineFile(audioFile, "caption.mp3");
            await this.bot.SendVoiceAsync(
                chatId,
                voice: inputOnlineFile,
                caption);
        }

        private async Task SendReplyKeyboard(long chatId, string[] answerOptions)
        {
            var keyboardButtons = new List<KeyboardButton[]> { };
            if (answerOptions.Length % 2 == 0)
            {
                for (int i = 0; i < answerOptions.Length / 2; i++)
                {
                    keyboardButtons.Add(new KeyboardButton[] { answerOptions[i * 2], answerOptions[(i * 2) + 1] });
                }
            }
            else
            {
                foreach (var answerOption in answerOptions)
                {
                    keyboardButtons.Add(new KeyboardButton[] { answerOption });
                }
            }

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                keyboardButtons.ToArray(),
                resizeKeyboard: true);

            await this.bot.SendTextMessageAsync(
                chatId: chatId,
                text: "Choose",
                replyMarkup: replyKeyboardMarkup);
        }
    }
}
