using Fias.Api.AutoMapperProfile;
using Fias.Api.Interfaces.Services;
using Fias.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapProfiler));

/*builder.Services.Configure<FormOptions>(options =>
    options.MultipartBodyLengthLimit = 268435456000
);

builder.Services.Configure<KestrelServerOptions>(options =>
    options.Limits.MaxRequestBodySize = 268435456000
);*/

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IXmlService, XmlService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
