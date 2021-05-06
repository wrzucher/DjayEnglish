namespace telegramBot
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Telegram.Bot;
    using Telegram.Bot.Exceptions;
    using Telegram.Bot.Extensions.Polling;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.InputFiles;
    using Telegram.Bot.Types.ReplyMarkups;

    public class TelegramBot
    {
        private readonly TelegramBotClient Bot;

        public TelegramBot(IConfiguration configuration)
        {
            var accessToken = configuration["TelegramBot:AccessToken"];
            this.Bot = new TelegramBotClient(accessToken);
        }

        public async Task Initialize()
        {
            var me = await this.Bot.GetMeAsync();
            Console.Title = me.Username;

            var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            this.Bot.StartReceiving(
                    new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                    cts.Token
                );
        }
        
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => this.BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage => this.BotOnMessageReceived(update.Message),
                UpdateType.CallbackQuery => this.BotOnCallbackQueryReceived(update.CallbackQuery),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            Console.WriteLine($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            var action = (message.Text.Split(' ').First()) switch
            {
                "/keyboard" => SendReplyKeyboard(message),
                "/photo" => SendAudioFile(message),
                _ => Usage(message)
            };
            await action;

            async Task SendReplyKeyboard(Message message)
            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                    },
                    resizeKeyboard: true
                );

                await this.Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: replyKeyboardMarkup
                );
            }

            async Task SendAudioFile(Message message)
            {
                await this.Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"tts.wav";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
                await this.Bot.SendAudioAsync(
                    chatId: message.Chat.Id,
                    audio: new InputOnlineFile(fileStream, fileName),
                    caption: "Nice Picture"
                );
            }

            async Task Usage(Message message)
            {
                const string usage = "Usage:\n" +
                                        "/keyboard - send custom keyboard\n" +
                                        "/photo    - send a photo\n";
                await this.Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: usage,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }
        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            await this.Bot.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}"
            );

            await this.Bot.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}"
            );
        }

        private static async Task UnknownUpdateHandlerAsync(Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
        }
    }

}
