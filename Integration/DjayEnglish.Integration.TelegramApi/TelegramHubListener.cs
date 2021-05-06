// -----------------------------------------------------------------------
// <copyright file="TelegramHubListener.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Integration.TelegramApi
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DjayEnglish.Integration.TelegramApi.ObjectModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Telegram.Bot;
    using Telegram.Bot.Exceptions;
    using Telegram.Bot.Extensions.Polling;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    /// <summary>
    /// Telegram hub listener.
    /// </summary>
    public class TelegramHubListener
    {
        private readonly ILogger<TelegramHubListener> logger;
        private readonly TelegramBotClient bot;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramHubListener"/> class.
        /// </summary>
        /// <param name="logger">Logger to use in current class.</param>
        /// <param name="configuration">Current App configuration.</param>
        public TelegramHubListener(
            ILogger<TelegramHubListener> logger,
            IConfiguration configuration)
        {
            var accessToken = configuration["TelegramBot:AccessToken"];
            this.bot = new TelegramBotClient(accessToken);
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Notify all listeners subscribed that quize should be started.
        /// </summary>
        public event EventHandler<OnUserStartQuizeEventArgs>? OnQuizeStart;

        /// <summary>
        /// Notify all listeners subscribed that user answer recived.
        /// </summary>
        public event EventHandler<OnUserAnswerRecivedEventArgs>? OnAnswerRecived;

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
                case "/quize": this.OnQuizeStart?.Invoke(this, new OnUserStartQuizeEventArgs(chatId));
                    return;
            }

            if (!action.StartsWith("/"))
            {
                this.OnAnswerRecived?.Invoke(this, new OnUserAnswerRecivedEventArgs(chatId, action));
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
    }
}
