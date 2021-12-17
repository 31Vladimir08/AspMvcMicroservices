using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(
                        config =>
                        {
                            config.AddJsonFile("appsettings.json", true);
                        })
                    .UseStartup<Startup>();
                })
                .ConfigureLogging((context, builder) 
                    =>
                    {
                        builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                        builder.AddFile();
                    });
        }
}
