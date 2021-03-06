// -----------------------------------------------------------------------
// <copyright file="CommandProcessingService.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App
{
    using System;
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
    /// Command processing service class.
    /// </summary>
    public class CommandProcessingService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<CommandProcessingService> logger;

        /// <summary>
        /// Timer which process start quiz start command.
        /// </summary>
        private Timer? processStartQuizQueueTimer;

        /// <summary>
        /// Timer which process answer recived command.
        /// </summary>
        private Timer? processAnswerRecivedQueueTimer;

        /// <summary>
        /// Timer which process quiz started command.
        /// </summary>
        private Timer? processQuizStartedQueueTimer;

        /// <summary>
        /// Timer which process answer result command.
        /// </summary>
        private Timer? processAnswerResultQueueTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessingService"/> class.
        /// </summary>
        /// <param name="scopeFactory">Scope factory for creation of services.</param>
        /// <param name="logger">Logger for use.</param>
        public CommandProcessingService(
            IServiceScopeFactory scopeFactory,
            ILogger<CommandProcessingService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        /// <summary>
        /// Start processing.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representing the asynchronous operation.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.processStartQuizQueueTimer = new Timer(
                this.ProcessStartQuizQueue,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1));
            this.processAnswerRecivedQueueTimer = new Timer(
                this.ProcessAnswerRecivedQueue,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1));
            this.processQuizStartedQueueTimer = new Timer(
                this.ProcessQuizStartedQueue,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1));
            this.processAnswerResultQueueTimer = new Timer(
                this.ProcessAnswerResultQueue,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop processing.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representing the asynchronous operation.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.processStartQuizQueueTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void ProcessStartQuizQueue(object? state)
        {
            try
            {
                do
                {
                    var betPeek = CommandQueueService.StartQuizQueue.TryDequeue(out var startQuizEventArgs);
                    if (betPeek && startQuizEventArgs != null)
                    {
                        await this.ProcessStartQuiz(startQuizEventArgs);
                    }
                }
                while (!CommandQueueService.StartQuizQueue.IsEmpty);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occured {nameof(this.ProcessStartQuizQueue)}");
            }
        }

        private async void ProcessAnswerRecivedQueue(object? state)
        {
            try
            {
                do
                {
                    var betPeek = CommandQueueService.AnswerRecivedQueue.TryDequeue(out var answerRecivedEventArgs);
                    if (betPeek && answerRecivedEventArgs != null)
                    {
                        await this.ProcessAnswerRecived(answerRecivedEventArgs);
                    }
                }
                while (!CommandQueueService.AnswerRecivedQueue.IsEmpty);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occured {nameof(this.ProcessStartQuizQueue)}");
            }
        }

        private async void ProcessQuizStartedQueue(object? state)
        {
            try
            {
                do
                {
                    var betPeek = CommandQueueService.QuizStartedQueue.TryDequeue(out var quizStartedEventArgs);
                    if (betPeek && quizStartedEventArgs != null)
                    {
                        await this.ProcessQuizStarted(quizStartedEventArgs);
                    }
                }
                while (!CommandQueueService.QuizStartedQueue.IsEmpty);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occured {nameof(this.ProcessStartQuizQueue)}");
            }
        }

        private async void ProcessAnswerResultQueue(object? state)
        {
            try
            {
                do
                {
                    var betPeek = CommandQueueService.AnswerResultQueue.TryDequeue(out var answerResultEventArgs);
                    if (betPeek && answerResultEventArgs != null)
                    {
                        await this.ProcessAnswerResult(answerResultEventArgs);
                    }
                }
                while (!CommandQueueService.AnswerResultQueue.IsEmpty);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occured {nameof(this.ProcessStartQuizQueue)}");
            }
        }

        private Task ProcessStartQuiz(OnUserStartQuizEventArgs eventArgs)
        {
            using var scope = this.scopeFactory.CreateScope();
            var quizManager = scope.ServiceProvider.GetRequiredService<QuizManager>();
            quizManager.StartQuiz(eventArgs.ChatId, DateTimeOffset.UtcNow);
            return Task.CompletedTask;
        }

        private Task ProcessAnswerRecived(OnUserAnswerRecivedEventArgs eventArgs)
        {
            using var scope = this.scopeFactory.CreateScope();
            var quizManager = scope.ServiceProvider.GetRequiredService<QuizManager>();
            quizManager.RegisterUserAnswer(eventArgs.ChatId, eventArgs.Answer, DateTimeOffset.UtcNow);
            return Task.CompletedTask;
        }

        private async Task ProcessQuizStarted(OnQuizStartedEventArgs eventArgs)
        {
            using var scope = this.scopeFactory.CreateScope();
            var telegramHubSender = scope.ServiceProvider.GetRequiredService<TelegramHubSender>();
            await telegramHubSender.SendQuizAsync(eventArgs.ChatId, eventArgs.Quiz).ConfigureAwait(false);
        }

        private async Task ProcessAnswerResult(OnUserAnswerResultEventArgs eventArgs)
        {
            using var scope = this.scopeFactory.CreateScope();
            var telegramHubSender = scope.ServiceProvider.GetRequiredService<TelegramHubSender>();
            await telegramHubSender.SendAnswerResultAsync(eventArgs.ChatId, eventArgs.IsAnswerRight).ConfigureAwait(false);
        }
    }
}