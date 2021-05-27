// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App
{
    using System.Collections.Generic;
    using AutoMapper;
    using DjayEnglish.EntityFramework;
    using DjayEnglish.Integration.TelegramApi;
    using DjayEnglish.Server.Core;
    using DjayEnglish.Server.Core.EntityFrameworkCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// Startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Current App configuration to use.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets current App configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection to use.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Profile, MappingProfile>();
            services.AddSingleton<Profile, EntityFrameworkMappingProfile>();

            services.AddSingleton<AutoMapper.IConfigurationProvider>(serviceProvider =>
            {
                var profiles = serviceProvider.GetRequiredService<IEnumerable<Profile>>();
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    foreach (var profile in profiles)
                    {
                        mc.AddProfile(profile);
                    }
                });
                return mappingConfig;
            });
            services.AddScoped<IMapper, Mapper>();

            var connectionString = this.Configuration.GetConnectionString("DjayEnglishDb");
            services.AddDbContext<DjayEnglishDBContext>(
                options => options.UseSqlServer(connectionString));

            services.AddSingleton<TelegramHubListener>();
            services.AddSingleton<QuizManagerEvents>();
            services.AddScoped<DbQuizPersistence>();
            services.AddScoped<TelegramHubSender>();
            services.AddScoped<RemoteServiceAudioBuilder>();
            services.AddScoped<IAudioProvider, LocalAudioProvider>();
            services.AddScoped<QuizManager>();

            services.AddHostedService<AudioBuilderService>();
            services.AddHostedService<CommandQueueService>();
            services.AddHostedService<TelegramHubInitializerService>();
            services.AddHostedService<CommandProcessingService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DjayEnglish.App", Version = "v1" });
            });

            var builder = services.AddMvc();
#if DEBUG
            builder.AddRazorRuntimeCompilation();
#endif
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder to use.</param>
        /// <param name="env">Web host environment to use.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DjayEnglish.App v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
