using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services;
using Pilot.Receiver.Consumers.CompanyConsumer;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Repository;
using Pilot.Receiver.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddScoped<ICompany, CompanyRepository>();
services.AddScoped<ICompanyUser, CompanyUserRepository>();
services.AddScoped<IFile, FileRepository>();
services.AddScoped<IHistoryAction, HistoryActionRepository>();
services.AddScoped<IMessage, MessageRepository>();
services.AddScoped<IProject, ProjectRepository>();
services.AddScoped<IProjectTask, ProjectTaskRepository>();
services.AddScoped<ITeam, TeamRepository>();

services.AddScoped<IValidatorService, ValidatorService>();

services.AddScoped<IUserService, UserService>();

// var mongoConfiguration = configuration.GetSection("MongoDatabase").Get<MongoConfig>()!;
// builder.Services.AddSingleton(
//     new MongoClient(mongoConfiguration.ConnectionString).GetDatabase(mongoConfiguration.DbName));


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


services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    // x.AddConsumers(Assembly.GetAssembly(typeof(IConsumer)));
    x.AddConsumer<CompanyCreatedConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(configuration.GetConnection("RabbitMQ:ConnectionString"));
        cfg.ConfigureEndpoints(ctx);
    });
});

services.AddDbContext<DataContext>(option => option.UseMySql(
        configuration.GetConnection("MySql:ConnectionString"),
        new MySqlServerVersion(new Version(8, 0, 11))
    )
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnection("RedisCache:ConnectionString");
    options.InstanceName = configuration.GetSection("RedisCache").GetValue<string>("InstanceName");
});

services.AddAutoMapper(typeof(AutoMapperProfile));

// TODO обобщить
services.AddHttpClient("IdentityServer", c =>
{
    c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!);
});

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Main page!");

app.MapControllers();
app.UseHttpsRedirection();

app.Run();

namespace Pilot.Receiver
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program;
}
