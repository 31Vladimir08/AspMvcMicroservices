using System;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Sinks.Elasticsearch;

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
                .UseSerilog((context, configuration) =>
                {
                    configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(
                            new Uri(context.Configuration["ElasticConfiguration:Url"]))
                        {
                            IndexFormat = $"applogs-{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".","-")}-logs-{DateTime.UtcNow:yyyy-MM}",
                            AutoRegisterTemplate= true,
                            NumberOfReplicas = 1,
                            NumberOfShards = 2,
                        })
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(
                        config =>
                        {
                            config.AddJsonFile("appsettings.json", true);
                        })
                    .UseStartup<Startup>();
                });
    }
}
