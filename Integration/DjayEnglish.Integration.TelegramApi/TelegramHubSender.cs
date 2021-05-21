// -----------------------------------------------------------------------
// <copyright file="TelegramHubSender.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Integration.TelegramApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DjayEnglish.Server.Core;
    using DjayEnglish.Server.ObjectModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Telegram.Bot;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.InputFiles;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Telegram bot sender.
    /// </summary>
    public class TelegramHubSender
    {
        private readonly ILogger<TelegramHubSender> logger;
        private readonly TelegramBotClient bot;
        private readonly IAudioProvider audioProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramHubSender"/> class.
        /// </summary>
        /// <param name="logger">Logger to use in current class.</param>
        /// <param name="configuration">Current App configuration.</param>
        /// <param name="audioProvider">Audio file provider.</param>
        public TelegramHubSender(
            ILogger<TelegramHubSender> logger,
            IConfiguration configuration,
            IAudioProvider audioProvider)
        {
            var accessToken = configuration["TelegramBot:AccessToken"];
            this.bot = new TelegramBotClient(accessToken);
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.audioProvider = audioProvider ?? throw new ArgumentNullException(nameof(audioProvider));
        }

        /// <summary>
        /// Send result of user answer back to the user.
        /// </summary>
        /// <param name="chatId">Id of the user chat.</param>
        /// <param name="isAnswerRight">Indicate than user answer was right.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SendAnswerResultAsync(long chatId, bool isAnswerRight)
        {
            if (isAnswerRight)
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

        /// <summary>
        /// Send quiz to user.
        /// </summary>
        /// <param name="chatId">Id of the user chat.</param>
        /// <param name="quiz">Model with information about quiz.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SendQuizeAsync(long chatId, Quiz quiz)
        {
            var question = quiz.GetQuizeText();
            var examples = quiz.GetExamplesText();
            var answerOptions = quiz.GetAnswerOptionsText();
            var showedAnswerOptions = quiz.GetShowedQuizeAnswerOptions();

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
                text: "choose the correct answer:",
                replyMarkup: replyKeyboardMarkup);
        }
    }
}
