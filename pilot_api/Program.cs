using MassTransit;
using MassTransit.Courier;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using pilot_api.Data;
using pilot_api.Models;
using pilot_api.Repository;
using Serilog;
using pilot_api.Queries;
using pilot_api.Repository;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

services.AddSingleton<CompanyRepository>();

var configuration = builder.Configuration;

builder.Services.AddSingleton(
    new MongoClient(configuration.GetConnectionString("MongoDb")
    ).GetDatabase(configuration["MongoDatabase"]));

services.AddSingleton<Serilog.ILogger>(_ => new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.File(configuration["Logging:LogFiles:Main"]!,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger()
);

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddEndpointsApiExplorer();
services.AddMassTransit(x =>
{
    var rabbitMqConfig = configuration.GetSection("RabbitMQ");
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"]);
            h.Password(rabbitMqConfig["Password"]);
        });
    });
});

var app = builder.Build();

await Seed.Seeding(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
