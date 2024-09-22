using System.Text;
using MassTransit;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Pilot.Api.Data;
using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Api.Interfaces;
using Pilot.Api.Services;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ApiExceptions;
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

services.AddHttpClient(ServiceName.WorkerServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("ReceiverServerUrl")!); });

services.AddHttpClient(ServiceName.MessengerServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("MessengerServerUrl")!); });

services.AddHttpClient(ServiceName.StorageServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("StorageServerUrl")!); });

services.AddHttpClient(ServiceName.CapabilityServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("CapabilityServerUrl")!); });

services.AddScoped<IModelService, ModelService>();
services.AddScoped<IBaseHttpService, BaseHttpService>();
services.AddScoped<IHttpIdentityService, HttpIdentityService>();
services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

await services.AddRedis(configuration);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger());

services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

// services.AddQueryHandlers(typeof(BaseDto).Assembly);

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
        };
    });

services.AddAuthorization();

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

await app.Services.GetRequiredService<ISeed>().Seeding();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Main api page!");

app.Run();

namespace Pilot.Api
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program;
}