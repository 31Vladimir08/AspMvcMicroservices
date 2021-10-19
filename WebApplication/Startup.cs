using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication.Models;

namespace WebApplication
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using DataAccessLayer;
    using DataAccessLayer.Interfaces;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using AutoMapperProfile;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var option = Configuration.GetSection(DbSettings.DbSettingsKey)
                .Get<DbSettings>();
            services.AddDbContext<AplicationDbContext>(options =>
                { 
                    options.UseSqlServer(option.ConnectionString);
                });
            services.AddAutoMapper(typeof(AutoMapProfiler), typeof(Startup));
            services.AddAutofac();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(Configuration["DbSettings:ConnectionString"]);
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder bilder)
        {
            bilder.RegisterType<AplicationDbContext>().As<IAplicationDbContext>();
        }
    }
}
