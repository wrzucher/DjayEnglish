// -----------------------------------------------------------------------
// <copyright file="CommandQueueService.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using DjayEnglish.Integration.TelegramApi;
    using DjayEnglish.Integration.TelegramApi.ObjectModels;
    using DjayEnglish.Server.Core;
    using DjayEnglish.Server.ObjectModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Queue service class.
    /// </summary>
    public class CommandQueueService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandQueueService"/> class.
        /// </summary>
        /// <param name="scopeFactory">Scope factory for creation of services.</param>
        public CommandQueueService(
            IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        /// <summary>
        /// Gets queue with start quiz command which need to process.
        /// </summary>
        public static ConcurrentQueue<OnUserStartQuizEventArgs> StartQuizQueue { get; } = new ConcurrentQueue<OnUserStartQuizEventArgs>();

        /// <summary>
        /// Gets queue with answer which need to process.
        /// </summary>
        public static ConcurrentQueue<OnUserAnswerRecivedEventArgs> AnswerRecivedQueue { get; } = new ConcurrentQueue<OnUserAnswerRecivedEventArgs>();

        /// <summary>
        /// Gets queue with started quiz which need to process.
        /// </summary>
        public static ConcurrentQueue<OnQuizStartedEventArgs> QuizStartedQueue { get; } = new ConcurrentQueue<OnQuizStartedEventArgs>();

        /// <summary>
        /// Gets queue with answer result which need to process.
        /// </summary>
        public static ConcurrentQueue<OnUserAnswerResultEventArgs> AnswerResultQueue { get; } = new ConcurrentQueue<OnUserAnswerResultEventArgs>();

        /// <summary>
        /// Start updating.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representing the asynchronous operation.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = this.scopeFactory.CreateScope();
            var telegramHubListener = scope.ServiceProvider.GetRequiredService<TelegramHubListener>();
            telegramHubListener.OnAnswerRecived += this.AnswerRecived;
            telegramHubListener.OnQuizStart += this.QuizStart;
            var quizManagerEvents = scope.ServiceProvider.GetRequiredService<QuizManagerEvents>();
            quizManagerEvents.OnQuizStarted += this.QuizStarted;
            quizManagerEvents.OnUserAnswerResultRecived += this.AnswerResultRecived;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop updating.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A representing the asynchronous operation.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void QuizStarted(object? sender, OnQuizStartedEventArgs eventArgs)
        {
            QuizStartedQueue.Enqueue(eventArgs);
        }

        private void AnswerResultRecived(object? sender, OnUserAnswerResultEventArgs eventArgs)
        {
            AnswerResultQueue.Enqueue(eventArgs);
        }

        private void AnswerRecived(object? sender, OnUserAnswerRecivedEventArgs eventArgs)
        {
            AnswerRecivedQueue.Enqueue(eventArgs);
        }

        private void QuizStart(object? sender, OnUserStartQuizEventArgs eventArgs)
        {
            StartQuizQueue.Enqueue(eventArgs);
        }
    }
}