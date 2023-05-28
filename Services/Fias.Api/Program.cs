using Fias.Api.AutoMapperProfile;
using Fias.Api.Contexts;
using Fias.Api.Extensions;
using Fias.Api.Interfaces.Repositories;
using Fias.Api.Interfaces.Services;
using Fias.Api.Models.Options.DataBase;
using Fias.Api.Repositories;
using Fias.Api.Services;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DbSettingsOption>(builder.Configuration.GetSection("DbSettings"));
builder.Services.AddAutoMapper(typeof(AutoMapProfiler));
//builder.Services.AddDbContextFactory<AppDbContext>();
builder.Services.AddDbContext<AppDbContext>();

//TODO надо чето с этой хуйней придумать
builder.Services.Configure<FormOptions>(options =>
    options.MultipartBodyLengthLimit = 268435456000
);
builder.Services.Configure<KestrelServerOptions>(options =>
    options.Limits.MaxRequestBodySize = 268435456000
);

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IXmlService, XmlService>();

builder.Services.AddScoped<IBaseRepository, BaseRepository>();

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
