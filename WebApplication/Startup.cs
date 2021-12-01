using System;
using System.IO;
using WebApplication.Extension;
using WebApplication.Filters;
using WebApplication.Interfaces;
using WebApplication.Services;
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
using WebApplication.AutoMapperProfile;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using WebApplication.Data;

namespace WebApplication
{
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
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<LogingCallsActionFilter>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddRazorPages();
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            // Register the Swagger services
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "ToDo API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Vladimir Dolidze",
                        Email = string.Empty,
                        Url = "https://vk.com/id52435997"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(Options.ConnectionString);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();

                //Enable middleware to serve swagger - ui assets(HTML, JS, CSS etc.)
                app.UseSwaggerUi3();
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
            
            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseCacheFile(x =>
            {
                x.SetParam(
                    Path.Combine(env.ContentRootPath, "wwwroot\\images"),
                    cacheExpirationTime: TimeSpan.FromMinutes(1));
            });
            app.UseAuthentication();    // аутентификация
            app.UseAuthorization();     // авторизация
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "images",
                    pattern: "images/{id?}",
                    defaults: new { controller = "Category", action = "GetPicture" });

                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
