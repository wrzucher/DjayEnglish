// <copyright file="TelegramQueueService.cs" company="LiveSoccer">
// Copyright (c) LiveSoccer. All rights reserved.
// </copyright>

namespace DjayEnglish.Integration.Telegram
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DjayEnglish.Core;
    using DjayEnglish.Integration.Telegram.ObjectModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Queue service class.
    /// </summary>
    public class TelegramQueueService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<TelegramQueueService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramQueueService"/> class.
        /// </summary>
        /// <param name="scopeFactory">Scope factory for creation of services.</param>
        /// <param name="logger">Logger for use.</param>
        public TelegramQueueService(
            IServiceScopeFactory scopeFactory,
            ILogger<TelegramQueueService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        /// <summary>
        /// Gets queue with start quize command which need to process.
        /// </summary>
        public static ConcurrentQueue<OnUserStartQuizeEventArgs> StartQuizeQueue { get; } = new ConcurrentQueue<OnUserStartQuizeEventArgs>();

        /// <summary>
        /// Gets queue with answer which need to process.
        /// </summary>
        public static ConcurrentQueue<OnUserAnswerRecivedEventArgs> AnswerRecivedQueue { get; } = new ConcurrentQueue<OnUserAnswerRecivedEventArgs>();

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
            telegramHubListener.OnQuizeStart += this.QuizeStart;
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

        private void AnswerRecived(object? sender, OnUserAnswerRecivedEventArgs eventArgs)
        {
        }

        private void QuizeStart(object sender, OnUserStartQuizeEventArgs eventArgs)
        {
        }
    }
}