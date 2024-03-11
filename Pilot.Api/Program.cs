using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using MongoDB.Driver;
using Pilot.Api.Data;
using Pilot.Api.Interfaces.Repositories;
using Pilot.Api.Repository;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

services.AddScoped<ICompany, CompanyRepository>();

services.AddHttpClient("IdentityServer", c =>
{
    c.BaseAddress = new Uri("https://localhost:7127");
});

var mongoConfiguration = configuration.GetSection("MongoDatabase").Get<MongoConfig>()!;
services.AddSingleton(
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
    .CreateLogger());

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

services.AddControllers(options =>
{
    // options.Filters.Add(new ApiExceptionFilter());
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddEndpointsApiExplorer();
services.AddMassTransit(x =>
{
    var rabbitMqConfig = configuration.GetSection("RabbitMQ");
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"]);
            h.Password(rabbitMqConfig["Password"]);
        });
    });
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

await app.Seeding();

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
