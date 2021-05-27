// -----------------------------------------------------------------------
// <copyright file="TelegramHubSender.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Integration.TelegramApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        public async Task SendQuizAsync(long chatId, Quiz quiz)
        {
            if (quiz.QuestionShowType == ShowType.Audio)
            {
                using var questionAudioFile = await this.audioProvider.GetAudio(quiz.Id, QuizePartType.Question).ConfigureAwait(false);
                await this.SendAudioFileAsync(chatId, "Question", questionAudioFile).ConfigureAwait(false);
            }

            if (quiz.ExampleShowType == ShowType.Audio)
            {
                using var usageAudioFile = await this.audioProvider.GetAudio(quiz.Id, QuizePartType.Usage).ConfigureAwait(false);
                await this.SendAudioFileAsync(chatId, "Usage", usageAudioFile).ConfigureAwait(false);
            }

            if (quiz.AnswerShowType == ShowType.Audio)
            {
                using var answersAudioFile = await this.audioProvider.GetAudio(quiz.Id, QuizePartType.AnswerOptions).ConfigureAwait(false);
                await this.SendAudioFileAsync(chatId, "Answer options", answersAudioFile).ConfigureAwait(false);
            }

            if (quiz.QuestionShowType == ShowType.Text)
            {
                var question = quiz.GetQuizText();
                await this.bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Question: {question}");
            }

            if (quiz.ExampleShowType == ShowType.Text)
            {
                var usage = quiz.GetExamplesText();
                await this.bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Usage: {usage}");
            }

            if (quiz.AnswerShowType == ShowType.Text)
            {
                var usage = quiz.GetAnswerOptionsText();
                await this.bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Answer options: \r\n{usage}");
            }

            var showedAnswerOptions = quiz.GetShowedQuizAnswerOptions();
            await this.SendReplyKeyboard(chatId, showedAnswerOptions).ConfigureAwait(false);
        }

        private async Task SendAudioFileAsync(
            long chatId,
            string caption,
            MemoryStream audioFile)
        {
            await this.bot.SendChatActionAsync(chatId, ChatAction.UploadAudio);
            var inputOnlineFile = new InputOnlineFile(audioFile, $"audio.wav");
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
