﻿using DaresGameBot.Web.Models;
using DaresGameBot.Web.Models.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedMember.Global

namespace DaresGameBot.Web
{
    public class Startup
    {
        public Startup(IConfiguration config) => _config = config;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IBot, Models.Bot>();
            services.AddHostedService<Service>();
            services.Configure<Config>(_config);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes => routes.MapRoute("update", $"{_config["Token"]}/{{controller=Update}}/{{action=post}}"));
        }

        private readonly IConfiguration _config;
    }
}