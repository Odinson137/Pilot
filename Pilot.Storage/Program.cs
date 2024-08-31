using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Storage.Consumers.Base;
using Pilot.Storage.Data;
using Pilot.Storage.Interface;
using Pilot.Storage.Repository;
using Pilot.Storage.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IStorageService, GoogleStorageService>();
services.AddScoped<IFileRepository, FileRepository>();

services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();
services.AddScoped<IMessageService, MessageService>();

services.AddDbContext<DataContext>(option => option.UseMySql(
        configuration["MySql:ConnectionString"],
        new MySqlServerVersion(new Version(8, 0, 11))
    )
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger());

services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    var baseModelType = typeof(BaseCreatedConsumer<,>);
    var assembly = Assembly.GetAssembly(baseModelType);

    var consumers = assembly!.GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } && t.Name.Contains("Consumer"))
        .Select(c => c)
        .ToList();

    foreach (var consumer in consumers) x.AddConsumer(consumer);

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(configuration["RabbitMQ:ConnectionString"]);
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", () => "Storage service!");

app.Run();

namespace Pilot.Storage
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program;
}