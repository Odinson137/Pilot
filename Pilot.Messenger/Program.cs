using MediatR;
using Pilot.Contracts.Base;
using Pilot.Messenger.Data;
using Pilot.Messenger.Hubs;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Repository;
using Pilot.Messenger.Services;
using Pilot.SqrsControllerLibrary;
using Pilot.SqrsControllerLibrary.Behaviors;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IBaseValidatorService, BaseValidateService>();
services.AddScoped<IModelService, ModelService>();
services.AddScoped<INotificationService, NotificationService>();

services.AddSignalR();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

builder.AddBaseServices<DataContext, Program, AutoMapperProfile>();

// services.AddBaseQueryHandlers(typeof(BaseDto).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<NotificationHub>("/notificationhub");
app.MapHub<ChatHub>("/chatHub");

app.UseHttpsRedirection();

app.MapGet("/", () => "Main messenger page!");

app.Run();