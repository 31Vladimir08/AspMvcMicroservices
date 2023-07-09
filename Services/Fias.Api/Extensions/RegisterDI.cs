using Fias.Api.HostedServices;
using Fias.Api.Interfaces.Services;
using Fias.Api.Services;

namespace Fias.Api.Extensions
{
    public static class RegisterDI
    {
        public static void RegisterInIoC(this IServiceCollection services)
        {
            services.SetServicesDJ();
        }

        public static void SetServicesDJ(this IServiceCollection services)
        {
            services.AddSingleton<FiasUpdateDbService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IXmlService, XmlService>();
        }
    }
}
