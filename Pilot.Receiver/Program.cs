using MassTransit;
using MongoDB.Driver;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Repository;
using Serilog;
using File = Pilot.Contracts.Models.File;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddScoped<ICompany, CompanyRepository>();
services.AddScoped<IBaseRepository<CompanyUser>, BaseRepository<CompanyUser>>();
services.AddScoped<IBaseRepository<File>, BaseRepository<File>>();
services.AddScoped<IBaseRepository<HistoryAction>, BaseRepository<HistoryAction>>();
services.AddScoped<IBaseRepository<Message>, BaseRepository<Message>>();
services.AddScoped<IBaseRepository<Project>, BaseRepository<Project>>();
services.AddScoped<IBaseRepository<ProjectTask>, BaseRepository<ProjectTask>>();
services.AddScoped<IBaseRepository<Team>, BaseRepository<Team>>();

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