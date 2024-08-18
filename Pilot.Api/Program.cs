using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Pilot.Api.Behaviors;
using Pilot.Api.Data;
using Pilot.Api.Interfaces;
using Pilot.Api.Services;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.InvalidationCacheRedisLibrary;
using Pilot.SqrsControllerLibrary.Behaviors;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

services.AddHttpClient(ServiceName.IdentityServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!); });

services.AddHttpClient(ServiceName.ReceiverServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("ReceiverServerUrl")!); });

services.AddHttpClient(ServiceName.MessengerServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("MessengerServerUrl")!); });

services.AddScoped<IModelService, ModelService>();
services.AddScoped<IBaseHttpService, BaseHttpService>();
services.AddScoped<IHttpIdentityService, HttpIdentityService>();
services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

// var mongoConfiguration = configuration.GetSection("MongoDatabase").Get<MongoConfig>()!;
// services.AddSingleton(
//     new MongoClient(mongoConfiguration.ConnectionString).GetDatabase(mongoConfiguration.DbName));

await services.AddRedis(configuration);

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


services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

services.AddQueryHandlers(typeof(BaseDto).Assembly);

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingOneBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryOneHandling<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CreateCommandHandling<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UpdateCommandHandling<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DeleteCommandHandling<,>));

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddEndpointsApiExplorer();
services.AddMassTransit(x =>
{
    x.UsingRabbitMq((_, cfg) => { cfg.Host(configuration["RabbitMQ:ConnectionString"]); });
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
    public class Program;
}