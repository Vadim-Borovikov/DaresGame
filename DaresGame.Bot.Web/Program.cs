using System;
using System.IO;
using DaresGame.Bot.Web.Models.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DaresGame.Bot.Web
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                IWebHost host = CreateWebHostBuilder(args).Build();

                using (IServiceScope scope = host.Services.CreateScope())
                {
                    IServiceProvider services = scope.ServiceProvider;
                    var db = services.GetRequiredService<DaresGameDbContext>();
                    DbInitializer.Initialize(db);
                }
                host.Run();
            }
            catch (Exception ex)
            {
                File.AppendAllText(LogPath, ex.ToString());
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((ctx, builder) =>
                {
                    builder.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                    builder.AddFile(o => o.RootPath = ctx.HostingEnvironment.ContentRootPath);
                })
                .UseStartup<Startup>();
        }

        private const string LogPath = "errors.txt";
    }
}
