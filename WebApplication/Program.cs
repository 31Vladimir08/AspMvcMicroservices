namespace WebApplication
{
    using Autofac.Extensions.DependencyInjection;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(
                        config =>
                        {
                            config.AddJsonFile("dbsettings.json", true);
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
