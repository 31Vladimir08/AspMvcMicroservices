using System.IO;
using System.Threading.Tasks;
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
    using System;

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
            services.Configure<DbSettings>(Configuration.GetSection(DbSettings.DbSettingsKey));
            services.AddDbContext<AplicationDbContext>(options =>
                { 
                    options.UseSqlServer(Options.ConnectionString);
                });
            services.AddAutoMapper(typeof(AutoMapProfiler), typeof(Startup));
            //services.AddAutofac();
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
                    Path.Combine(env.ContentRootPath, "wwwroot\\images"));
            });
            app.Use(async (context, next) =>
            {
                await next();
                string path = context?.Request?.Path.Value?.ToLower();

                if (!string.IsNullOrWhiteSpace(path) && path.Contains("/category/getpicture") && context.Request.Method == "POST")
                {
                    await SetFileAsync(context.Request.Body, env, path);
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async Task SetFileAsync(Stream uploadedFile, IWebHostEnvironment env, string path)
        {
            await Task.Run(async () =>
            {
                using (var mem = new MemoryStream())
                {
                    char[] arr = path.ToCharArray();
                    Array.Reverse(arr);
                    string name = new string(arr);
                    name = name.Substring(0, name.IndexOf('/'));
                    arr = name.ToCharArray();
                    Array.Reverse(arr);
                    name = new string(arr);

                    using (var memoryStream = new FileStream($"{env.ContentRootPath}/wwwroot/images/{name}.png", FileMode.OpenOrCreate))
                    {
                        await uploadedFile.CopyToAsync(mem);
                        var array = new byte[memoryStream.Length];
                        await memoryStream.WriteAsync(array, 0, array.Length);
                        mem.Seek(0, SeekOrigin.Begin);
                    }
                }
            });
        }
    }
}
