using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Receiver.Consumers;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Repository;
using Pilot.Receiver.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<ICompany, CompanyRepository>();
services.AddScoped<ICompanyUser, CompanyUserRepository>();
services.AddScoped<IMessage, MessageService>();
services.AddScoped<IUserService, UserService>();

services.AddHttpClient("IdentityServer", c =>
{
    c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!);
});

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
    x.AddConsumer<CompanyUserCreatedConsumer>();
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"]);
            h.Password(rabbitMqConfig["Password"]);
        });
        
        cfg.ConfigureEndpoints(ctx);
    });
});

services.AddTransient<ISeed, Seed>();

var app = builder.Build();

await app.Services.GetRequiredService<ISeed>().Seeding(app);


app.UseSwagger();
app.UseSwaggerUI();

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

            var logger = app.Services.GetRequiredService<ILogger<IExceptionHandlerFeature>>();
            
            logger.LogClassInfo(context);
            
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(error.Error.Message);
        }
    });
});

app.UseHttpsRedirection();

app.MapGet("/", () => "Main page!");

app.MapGet("/Company", async (
    ILogger<Program> logger, 
    ICompany company,
    CancellationToken cancellationToken) =>
{
    logger.LogInformation("Get companies");

    var companies = await company.GetCompaniesAsync(cancellationToken);
    
    logger.LogInformation("Successfully  getting companies");

    return Results.Ok(companies);
});

app.MapGet("/Company/{companyId}", async (
    ILogger<Program> logger, 
    ICompany companyRepository,
    string companyId,
    CancellationToken cancellationToken) =>
{
    logger.LogInformation($"Get company by id {companyId}");

    var company = await companyRepository.GetCompanyAsync(companyId, cancellationToken);
    
    logger.LogInformation("Successfully  getting companies");

    return Results.Ok(company);
});

app.MapGet("/CompanyUser/{companyId}", async (
    ILogger<Program> logger, 
    ICompany companyRepository,
    string companyId,
    CancellationToken cancellationToken) =>
{
    logger.LogInformation($"Get all user in company by companyId {companyId}");

    var users = await companyRepository.GetCompanyAsync(companyId, cancellationToken);
    
    logger.LogInformation("Successfully getting company users");

    return Results.Ok(users);
});

app.MapGet("/Project/{companyId}", async (
    ILogger<Program> logger, 
    IProject projectRepository,
    string companyId,
    CancellationToken cancellationToken) =>
{
    logger.LogInformation($"Get all projects in company by companyId {companyId}");

    var users = await projectRepository.GetCompanyProjectsAsync(companyId, cancellationToken);
    
    logger.LogInformation("Successfully getting company projects");

    return Results.Ok(users);
});

app.MapGet("/Project/{companyId}/{projectId}", async (
    ILogger<Program> logger, 
    IProject projectRepository,
    string companyId,
    string projectId,
    CancellationToken cancellationToken) =>
{
    logger.LogInformation($"Get project in company by companyId {companyId} and by projectId {projectId}");

    var users = await projectRepository.GetCompanyProjectByIdAsync(companyId, projectId, cancellationToken);
    
    logger.LogInformation("Successfully getting project");

    return Results.Ok(users);
});



app.Run();

namespace Pilot.Receiver
{
    public partial class Program {}
}
