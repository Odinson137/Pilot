using System.Reflection;
using System.Text;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Pilot.Api.Behaviors;
using Pilot.Api.Data;
using Pilot.Api.Handlers;
using Pilot.Api.Interfaces.Repositories;
using Pilot.Api.Repository;
using Pilot.Contracts.Data;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

services.AddScoped<ICompany, CompanyRepository>();
services.AddScoped<ICompanyUser, CompanyUserRepository>();

services.AddHttpClient("IdentityServer", c =>
{
    c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!);
});

var mongoConfiguration = configuration.GetSection("MongoDatabase").Get<MongoConfig>()!;
services.AddSingleton(
    new MongoClient(mongoConfiguration.ConnectionString).GetDatabase(mongoConfiguration.DbName));

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetSection("RedisCache").GetValue<string>("ConnectionString");
    options.InstanceName = configuration.GetSection("RedisCache").GetValue<string>("InstanceName");
});

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Logger(lc =>
    {
        lc.MinimumLevel.Error();
        lc.WriteTo.MongoDb(mongoConfiguration);
    })
    .CreateLogger());

services.AddControllers();

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddEndpointsApiExplorer();
services.AddMassTransit(x =>
{
    var rabbitMqConfig = configuration.GetSection("RabbitMQ");
    
    x.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"]);
            h.Password(rabbitMqConfig["Password"]);
        });
    });
});

services.AddMemoryCache();

services.AddTransient<ISeed, Seed>();

services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // TODO
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.Key)),
            ValidIssuer = Jwt.Issuer,
        };
    });

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

