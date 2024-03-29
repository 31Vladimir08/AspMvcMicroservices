using Common.Logging;

using EmailService.Entities.Settings;
using EmailService.EventBusConsumer;
using EmailService.Interfaces.Services;
using EmailService.Services;

using EventBus.Messages.Common;

using MassTransit;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddMassTransit(config => {
    config.AddConsumer<EmailConsumer>();
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstants.EmailConfirmationOfRegistrationQueue, c => {
            c.ConfigureConsumer<EmailConsumer>(ctx);
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IEmailServices, EmailServices>();

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
