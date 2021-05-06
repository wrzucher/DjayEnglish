using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace telegramBot
{
    class Program
    {
        
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);
            using var host = builder.Build();

            using var scope = host.Services.CreateScope();
            var telegramBot = scope.ServiceProvider.GetService<TelegramBot>();
            await telegramBot.Initialize().ConfigureAwait(false);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TelegramBot>();
        }
    }
}
