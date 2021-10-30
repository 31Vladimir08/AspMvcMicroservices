using System.IO;
using WebApplication.Extension;

namespace WebApplication
{
    using System.Net;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using WebApplication.Models;

    using DataAccessLayer;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using AutoMapperProfile;
    using DataAccessLayer.Interfaces;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private DbSettings Options { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Options = Configuration.GetSection(DbSettings.DbSettingsKey)
                .Get<DbSettings>();
            services.AddMemoryCache();
            services.Configure<DbSettings>(Configuration.GetSection(DbSettings.DbSettingsKey));
            services.AddDbContext<AplicationDbContext>(options =>
                { 
                    options.UseSqlServer(Options.ConnectionString);
                });
            services.AddAutoMapper(typeof(AutoMapProfiler), typeof(Startup));
            services.AddControllersWithViews();
            services.AddScoped<IAplicationDbContext, AplicationDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(Options.ConnectionString);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; ;
                        context.Response.ContentType = "text/html";

                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        logger.LogError(exceptionHandlerPathFeature?.Error.Message);

                        if (exceptionHandlerPathFeature?.Error.Message != null)
                            await context.Response.WriteAsync(exceptionHandlerPathFeature?.Error.Message);
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCacheFile(x =>
            {
                x.SetParam(
                    Path.Combine(env.ContentRootPath, "wwwroot\\images"),
                    minutes: 5);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
