// <copyright file="TelegramProcessingService.cs" company="LiveSoccer">
// Copyright (c) LiveSoccer. All rights reserved.
// </copyright>

namespace DjayEnglish.Integration.Telegram
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Command processing service class.
    /// </summary>
    public class TelegramProcessingService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<TelegramProcessingService> logger;

        /// <summary>
        /// Timer updater.
        /// </summary>
        private Timer? updater;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramProcessingService"/> class.
        /// </summary>
        /// <param name="scopeFactory">Scope factory for creation of services.</param>
        /// <param name="logger">Logger for use.</param>
        public TelegramProcessingService(
            IServiceScopeFactory scopeFactory,
            ILogger<TelegramProcessingService> logger)
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
            this.updater = new Timer(
                this.ProcessBets,
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
            this.updater?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void ProcessBets(object? state)
        {
            try
            {
                do
                {
                    /* var betPeek = TelegramQueueService.BetsQueue.TryDequeue(out var gameBet);
                    if (betPeek && gameBet != null)
                    {
                        await this.ProcessBet(partnerClient, gameBet, DateTimeOffset.UtcNow);
                    }
                    */
                }
                while (!TelegramQueueService.StartQuizeQueue.IsEmpty);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occured during bets processing.");
            }
        }

        /*
        private async Task ProcessBet(, DateTimeOffset currentDate)
        {
            using var scope = this.scopeFactory.CreateScope();
        }
        */
    }
}