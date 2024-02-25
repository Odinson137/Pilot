using MassTransit;
using pilot_receiver.Consumers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddSingleton<Serilog.ILogger>(_ => new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.File(configuration["Logging:LogFiles:Main"]!,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger()
);


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

// services.AddMassTransitHostedService();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();