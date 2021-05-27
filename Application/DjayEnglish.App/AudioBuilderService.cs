// -----------------------------------------------------------------------
// <copyright file="AudioBuilderService.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using DjayEnglish.Integration.TelegramApi;
    using DjayEnglish.Integration.TelegramApi.ObjectModels;
    using DjayEnglish.Server.Core;
    using DjayEnglish.Server.ObjectModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Audio bulder service class.
    /// </summary>
    public class AudioBuilderService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<AudioBuilderService> logger;
        private Timer? updater;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBuilderService"/> class.
        /// </summary>
        /// <param name="scopeFactory">Scope factory for creation of services.</param>
        /// <param name="logger">Logger for current class.</param>
        public AudioBuilderService(
            IServiceScopeFactory scopeFactory,
            ILogger<AudioBuilderService> logger)
        {
            this.scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.updater = new Timer(
                this.ProcessQuizzes,
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static async Task SaveAudioToFile(
            RemoteServiceAudioBuilder audioProvider,
            string filePath,
            string text)
        {
            using var audioStream = await audioProvider.GetAudio(text).ConfigureAwait(false);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            audioStream.CopyTo(fileStream);
        }

        private async void ProcessQuizzes(object? sender)
        {
            using var scope = this.scopeFactory.CreateScope();
            var quizManager = scope.ServiceProvider.GetRequiredService<QuizManager>();
            var audioProvider = scope.ServiceProvider.GetRequiredService<RemoteServiceAudioBuilder>();
            try
            {
                var quizzesWithoutAudio = quizManager.GetQuizzesWithoutAudio();
                foreach (var quiz in quizzesWithoutAudio)
                {
                    var directory = $"media/{quiz.Id}";
                    Directory.CreateDirectory(directory);
                    if (quiz.ExampleShowType == ShowType.Audio)
                    {
                        var fileName = $"{QuizePartType.Usage.ToString()}.wav";
                        var fullFilePath = Path.Combine(directory, fileName);
                        await SaveAudioToFile(audioProvider, fullFilePath, quiz.GetExamplesText()).ConfigureAwait(false);
                    }

                    if (quiz.QuestionShowType == ShowType.Audio)
                    {
                        var fileName = $"{QuizePartType.Question.ToString()}.wav";
                        var fullFilePath = Path.Combine(directory, fileName);
                        await SaveAudioToFile(audioProvider, fullFilePath, quiz.GetQuizText()).ConfigureAwait(false);
                    }

                    if (quiz.AnswerShowType == ShowType.Audio)
                    {
                        var fileName = $"{QuizePartType.AnswerOptions.ToString()}.wav";
                        var fullFilePath = Path.Combine(directory, fileName);
                        await SaveAudioToFile(audioProvider, fullFilePath, quiz.GetAnswerOptionsText()).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occured in {nameof(AudioBuilderService)}");
            }
        }
    }
}