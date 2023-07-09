using Fias.Api;
using Fias.Api.AutoMapperProfile;
using Fias.Api.Contexts;
using Fias.Api.Extensions;
using Fias.Api.Filters;
using Fias.Api.HostedServices;
using Fias.Api.Models.Options.DataBase;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);
Start.DeleteTempDirectory();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DbSettingsOption>(builder.Configuration.GetSection("DbSettings"));
builder.Services.AddAutoMapper(typeof(AutoMapProfiler));
builder.Services.AddDbContextFactory<AppDbContext>();


builder.Services.Configure<FormOptions>(options =>
    options.MultipartBodyLengthLimit = 268435456000
);
builder.Services.Configure<KestrelServerOptions>(options =>
    options.Limits.MaxRequestBodySize = 268435456000
);

builder.Services.AddScoped<UploadCallsActionFilter>();
builder.Services.RegisterInIoC();
builder.Services.AddHostedService(provider => provider.GetService<FiasUpdateDbService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseException();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
