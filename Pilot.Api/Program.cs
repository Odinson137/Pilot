using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Pilot.Api.Behaviors;
using Pilot.Api.Data;
using Pilot.Api.Services;
using Pilot.Contracts.Data;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// services.Configure<RabbitMqConfigure>(configuration.GetSection("RabbitMQ"));

services.AddHttpClient("IdentityServer", c =>
{
    c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!);
});

services.AddHttpClient("ReceiverServer", c =>
{
    c.BaseAddress = new Uri(configuration.GetValue<string>("ReceiverServerUrl")!);
});

services.AddScoped<IHttpIdentityService, HttpIdentityService>();
services.AddScoped<IHttpReceiverService, HttpReceiverService>();

// var mongoConfiguration = configuration.GetSection("MongoDatabase").Get<MongoConfig>()!;
// services.AddSingleton(
//     new MongoClient(mongoConfiguration.ConnectionString).GetDatabase(mongoConfiguration.DbName));

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnection("RedisCache:ConnectionString");
    options.InstanceName = configuration.GetSection("RedisCache").GetValue<string>("InstanceName");
});

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    // .WriteTo.Logger(lc =>
    // {
    //     lc.MinimumLevel.Error();
    //     lc.WriteTo.MongoDb(mongoConfiguration);
    // })
    .CreateLogger());


services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    // cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    // cfg.AddBehavior(typeof(LoggingBehavior<,>));
});

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryListHandling<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryOneHandling<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CreateCommandHandling<,>));

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddEndpointsApiExplorer();
services.AddMassTransit(x =>
{
    x.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(configuration.GetConnection("RabbitMQ:ConnectionString"));
    });
});

services.AddTransient<ISeed, Seed>();
services.AddTransient<IToken, TokenService>();

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

await app.Services.GetRequiredService<ISeed>().Seeding(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

namespace Pilot.Api
{
    public partial class Program {}
}

