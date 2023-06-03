using Fias.Api.HostedServices;
using Fias.Api.Interfaces.Repositories;
using Fias.Api.Interfaces.Services;
using Fias.Api.Repositories;
using Fias.Api.Services;

namespace Fias.Api.Exceptions
{
    public static class RegisterDI
    {
        public static void RegisterInIoC(this IServiceCollection services)
        {
            services.SetRepositoriesDJ();
            services.SetServicesDJ();
        }

        public static void SetServicesDJ(this IServiceCollection services)
        {
            services.AddSingleton<FiasUpdateDbService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IXmlService, XmlService>();
        }

        public static void SetRepositoriesDJ(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository, BaseRepository>();
        }
    }
}
