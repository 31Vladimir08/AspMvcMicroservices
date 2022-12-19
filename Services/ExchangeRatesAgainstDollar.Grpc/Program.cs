using ExchangeRatesAgainstDollar.Grpc.Models.Settings;
using ExchangeRatesAgainstDollar.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddFile();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.Configure<WebCurrensySettings>(builder.Configuration.GetSection("WebCurrensySettings"));
builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ExchangeRateService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
