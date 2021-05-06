// -----------------------------------------------------------------------
// <copyright file="TelegramHubInitializerService.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Integration.TelegramApi
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Service class for telegram hub initializing.
    /// </summary>
    public class TelegramHubInitializerService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<TelegramHubInitializerService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramHubInitializerService"/> class.
        /// </summary>
        /// <param name="scopeFactory">Scope factory for creation of services.</param>
        /// <param name="logger">Logger for use.</param>
        public TelegramHubInitializerService(
            IServiceScopeFactory scopeFactory,
            ILogger<TelegramHubInitializerService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        /// <summary>
        /// Start service.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = this.scopeFactory.CreateScope();
            var telegramBot = scope.ServiceProvider.GetRequiredService<TelegramHubListener>();
            await telegramBot.InitializeAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Stop service.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = this.scopeFactory.CreateScope();
            var telegramBot = scope.ServiceProvider.GetRequiredService<TelegramHubListener>();
            await telegramBot.StopAsync().ConfigureAwait(false);
        }
    }
}