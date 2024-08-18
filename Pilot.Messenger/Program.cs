using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.InvalidationCacheRedisLibrary;
using Pilot.Messenger.Hubs;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Repository;
using Pilot.Messenger.Services;
using Pilot.SqrsControllerLibrary.Behaviors;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IBaseValidatorService, ValidatorService>();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<NotificationHub>("/notificationhub");
app.MapHub<ChatHub>("/chatHub");

app.UseHttpsRedirection();

app.Run();