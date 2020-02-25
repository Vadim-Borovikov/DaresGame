﻿using System;
using System.Threading.Tasks;
using DaresGame.Bot.Web.Logging;
using DaresGame.Bot.Web.Models;
using DaresGame.Bot.Web.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DaresGame.Bot.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IBotService, BotService>();
            services.AddHostedService<BotService>();
            services.Configure<BotConfiguration>(Configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            string path = $"{env.WebRootPath}{LogName}";
            loggerFactory.AddProvider(new FileLoggerProvider(path, LogLevel.Warning));
            ILogger<FileLogger> logger = loggerFactory.CreateLogger<FileLogger>();

            app.UseExceptionHandler(a => a.Run(c => HandleExceptionAsync(c, logger)));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes => routes.MapRoute("update", $"{Configuration["Token"]}/{{controller=Update}}/{{action=post}}"));
        }

        private static Task HandleExceptionAsync(HttpContext context, ILogger logger)
        {
            var feature = context.Features.Get<IExceptionHandlerPathFeature>();
            Exception exception = feature.Error;

            logger.LogError(exception, exception.ToString());
            return Task.CompletedTask;
        }

        private const string LogName = "log.txt";
    }
}