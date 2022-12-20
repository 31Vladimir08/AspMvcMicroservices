using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NUnit.Framework;

using WebApplication.Filters;
using WebApplication.Interfaces;
using WebApplication.Services;

namespace WebApplication.Integration.Tests
{
    public abstract class BaseTest
    {
        protected IHost Host { get; private set; }

        [SetUp]
        public async Task SetupBeforeEachTest()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration(
                    config =>
                    {
                        config.AddJsonFile("appsettings.json", true);
                    })
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseStartup<Startup>();
                    webHost.UseTestServer();
                    webHost.ConfigureServices((context, services) =>
                    {
                        services.AddScoped<IProductService, ProductService>();
                        services.AddScoped<ICategoryService, CategoryService>();
                        services.AddScoped<LogingCallsActionFilter>();
                    });
                });

            var host = await hostBuilder.StartAsync();

            Host = host;
        }

        [TearDown]
        public void Cleanup()
        {
            Host?.Dispose();
        }
    }
}
