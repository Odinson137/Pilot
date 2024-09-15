using System.Reflection;
using AutoMapper;
using MassTransit;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pilot.SqrsControllerLibrary.Interfaces;
using Pilot.SqrsControllerLibrary.Services;
using Serilog;

namespace Pilot.SqrsControllerLibrary;

public static class AddBaseService
{
    public static void AddBaseServices<TDb, TProgram, TMapper>(this WebApplicationBuilder builder) where TDb : DbContext where TMapper : Profile
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(TProgram).Assembly);
            cfg.NotificationPublisher = new TaskWhenAllPublisher();
            cfg.NotificationPublisherType = typeof(TaskWhenAllPublisher);
        });
        
        services.AddDbContext<TDb>(option => option.UseMySql(
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

            var baseModelType = typeof(TProgram);
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

        services.AddAutoMapper(typeof(TMapper));

        services.AddScoped<IFileUrlService, FileUrlService>();
    }
}