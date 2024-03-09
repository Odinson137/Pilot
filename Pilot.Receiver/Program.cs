using MassTransit;
using MongoDB.Driver;
using Pilot.Receiver.Consumers;
using Pilot.Contracts.Services.LogService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


var mongoConfiguration = configuration.GetSection("MongoDatabase").Get<MongoConfig>()!;
builder.Services.AddSingleton(
    new MongoClient(mongoConfiguration.ConnectionString).GetDatabase(mongoConfiguration.DbName));

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Logger(lc =>
    {
        lc.MinimumLevel.Error();
        lc.WriteTo.MongoDb(mongoConfiguration);
    })
    .WriteTo.File(configuration["Logging:LogFiles:Main"]!,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger());

services.AddMassTransit(x =>
{
    var rabbitMqConfig = configuration.GetSection("RabbitMQ");

    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<CompanyCreatedConsumer>();
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"]);
            h.Password(rabbitMqConfig["Password"]);
        });
        
        cfg.ConfigureEndpoints(ctx);

        
        // cfg.ReceiveEndpoint("message_queue", e =>
        // {
        //     e.ConfigureConsumer<CompanyCreatedConsumer>(ctx);
        // });
    });
});

var app = builder.Build();

app.MapGet("/", () => "Main page!");

app.Run();