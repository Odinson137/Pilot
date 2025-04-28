using System.Text;
using MassTransit;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Pilot.Api.Data;
using Pilot.Api.Interfaces;
using Pilot.Api.Services;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.SqrsControllerLibrary;
using Pilot.SqrsControllerLibrary.Behaviors;
using Pilot.SqrsControllerLibrary.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

services.AddHttpClient(nameof(ServiceName.IdentityServer),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!); });

services.AddHttpClient(nameof(ServiceName.WorkerServer),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("ReceiverServerUrl")!); });

services.AddHttpClient(nameof(ServiceName.MessengerServer),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("MessengerServerUrl")!); });

services.AddHttpClient(nameof(ServiceName.StorageServer),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("StorageServerUrl")!); });

services.AddHttpClient(nameof(ServiceName.CapabilityServer),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("CapabilityServerUrl")!); });

services.AddHttpClient(nameof(ServiceName.BackgroundJobService),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("BackgroundJobServiceUrl")!); });

services.AddScoped<IModelService, ModelService>();
services.AddScoped<IBaseHttpService, BaseHttpService>();
services.AddScoped<IHttpIdentityService, HttpIdentityService>();
services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

services.AddScoped<IFileService, FileService>();

services.AddRedis(configuration);

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.NotificationPublisher = new TaskWhenAllPublisher();
    cfg.NotificationPublisherType = typeof(TaskWhenAllPublisher);
});

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
// services.AddScoped(typeof(IPipelineBehavior<,>), typeof(HasFileBehavior<,>));
// services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ChangeFileBehavior<,>));

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
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(configuration["RabbitMQ:ConnectionString"]);
        cfg.ConfigureEndpoints(ctx);
    });
});

services.AddTransient<ISeed, Seed>();
services.AddTransient<IToken, TokenService>();

services.AddScoped<IFileService, FileService>();

builder.AddBaseLogService<Program>();

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
    public partial class Program;
}