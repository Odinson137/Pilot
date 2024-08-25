using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.InvalidationCacheRedisLibrary;
using Pilot.Messenger.Data;
using Pilot.Messenger.Hubs;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Repository;
using Pilot.Messenger.Services;
using Pilot.SqrsControllerLibrary.Behaviors;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHttpClient(ServiceName.IdentityServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!); });

services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IBaseValidatorService, ValidatorService>();
services.AddScoped<IModelService, ModelService>();
services.AddScoped<INotificationService, NotificationService>();

services.AddSignalR();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.NotificationPublisher = new TaskWhenAllPublisher();
    cfg.NotificationPublisherType = typeof(TaskWhenAllPublisher);
});

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingOneBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingListBehavior<,>));

await services.AddRedis(configuration);

services.AddDbContext<DataContext>(option => option.UseMySql(
        configuration["MySql:ConnectionString"],
        new MySqlServerVersion(new Version(8, 0, 11))
    )
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.MapHub<NotificationHub>("/notificationhub");
app.MapHub<ChatHub>("/chatHub");

app.UseHttpsRedirection();

app.MapGet("/", () => "Main messenger page!");

app.Run();