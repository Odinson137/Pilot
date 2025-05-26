using AutoMapper;
using MassTransit;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pilot.SqrsControllerLibrary.Interfaces;
using Pilot.SqrsControllerLibrary.Repositories;

namespace Pilot.SqrsControllerLibrary;

public static class AddBaseService
{
    public static void AddBaseServices<TDb, TMapper, TProgram>(this WebApplicationBuilder builder, Type[]? sagas = null, Tuple<string, Action<IBusRegistrationContext, IReceiveEndpointConfigurator>>[]? consumers = null)
        where TDb : DbContext where TMapper : Profile
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        var assembly = typeof(TProgram).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.NotificationPublisher = new TaskWhenAllPublisher();
            cfg.NotificationPublisherType = typeof(TaskWhenAllPublisher);
        });

        AddDbContext<TDb>(builder);

        builder.AddBaseLogService<TProgram>();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumers(assembly);
            
            // x.AddSagaStateMachine<JobApplicationSaga, JobApplicationSagaState>().RedisRepository(r =>
            // {
            //     r.ConnectionFactory(configuration["RedisCache:ConnectionString"]);
            //     r.Prefix("saga:");
            // });
            
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:ConnectionString"], h =>
                {
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Test")
                    {
                        h.UseCluster(c =>
                        {
                            c.Node("pilot-rabbitmq-1");
                            c.Node("pilot-rabbitmq-2");
                            c.Node("pilot-rabbitmq-3");
                        });   
                    }
                });
                if (consumers != null)
                {
                    foreach (var consumer in consumers)
                    {
                        cfg.ReceiveEndpoint(consumer.Item1, e => consumer.Item2(ctx, e));
                    }
                }
                cfg.ConfigureEndpoints(ctx);
            });

            foreach (var saga in sagas ?? [])
                x.AddSaga(saga);
        });

        services.AddAutoMapper(typeof(TMapper));
    }

    public static void AddUnitOfWork<TUoW>(this WebApplicationBuilder builder)
        where TUoW : BaseUnitOfWork
    {
        builder.Services.AddScoped<IUnitOfWork, TUoW>();
    }

    public static void AddBaseServices<TMapper, TProgram>(this WebApplicationBuilder builder)
        where TMapper : Profile
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        var assembly = typeof(TProgram).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.NotificationPublisher = new TaskWhenAllPublisher();
            cfg.NotificationPublisherType = typeof(TaskWhenAllPublisher);
        });

        builder.AddBaseLogService<TProgram>();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            var consumers = assembly.GetTypes()
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
    }

    public static void AddDbContext<TDbContext>(this WebApplicationBuilder builder, string connectionString = "MySql:ConnectionString")
        where TDbContext : DbContext
    {
        builder.Services.AddDbContext<TDbContext>(option => option.UseMySql(
                builder.Configuration[connectionString],
                new MySqlServerVersion(new Version(8, 0, 11))
            )
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
        );
    }
}