using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Contracts.Interfaces;
using Pilot.InvalidationCacheRedisLibrary;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Repository;
using Pilot.Receiver.Service;
using Serilog;
using IBaseValidatorService = Pilot.Contracts.Base.IBaseValidatorService;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddScoped<ICompany, CompanyRepository>();
services.AddScoped<ICompanyUser, CompanyUserRepository>();
services.AddScoped<IFile, FileRepository>();
services.AddScoped<IHistoryAction, HistoryActionRepository>();
services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IProject, ProjectRepository>();
services.AddScoped<IProjectTask, ProjectTaskRepository>();
services.AddScoped<ITeam, TeamRepository>();

services.AddScoped<IModelService, ModelService>();
services.AddScoped<IBaseValidatorService, ValidatorService>();

services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

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

services.AddDbContext<DataContext>(option => option.UseMySql(
        configuration["MySql:ConnectionString"],
        new MySqlServerVersion(new Version(8, 0, 11))
    )
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

await services.AddRedis(configuration);

services.AddAutoMapper(typeof(AutoMapperProfile));

services.AddHttpClient(ServiceName.IdentityServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!); });

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features.Get<IExceptionHandlerFeature>();

        if (error != null)
        {
            context.Response.StatusCode = error.Error switch
            {
                BadRequestException => 400,
                NotFoundException => 404,
                _ => 500
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(error.Error.Message);
        }
    });
});

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.MapGet("/", () => "Main receiver page!");

app.MapControllers();
app.UseHttpsRedirection();

app.Run();

namespace Pilot.Receiver
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program;
}