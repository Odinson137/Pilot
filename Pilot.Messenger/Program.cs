using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Interfaces;
using Pilot.Messenger.Data;
using Pilot.Messenger.Hubs;
using Pilot.Messenger.Interfaces;
using Pilot.Messenger.Repository;
using Pilot.Messenger.Services;
using Pilot.SqrsControllerLibrary;
using Pilot.SqrsControllerLibrary.Behaviors;
using Pilot.SqrsControllerLibrary.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHttpClient(nameof(ServiceName.WorkerServer),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("ReceiverServerUrl")!); });

services.AddHttpClient(nameof(ServiceName.CapabilityServer),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("CapabilityServerUrl")!); });

services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

services.AddScoped<IInfoMessageRepository, InfoMessageRepository>();
services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IChatRepository, ChatRepository>();
services.AddScoped<IChatMemberRepository, ChatMemberRepository>();
services.AddScoped<IBaseValidatorService, MessengerValidateService>();
services.AddScoped<INotificationService, NotificationService>();

services.AddSignalR();
services.AddRedis(configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddControllers();

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped<IModelService, ModelService>();

builder.AddBaseServices<DataContext, AutoMapperProfile, Program>();
services.AddRedis(configuration);

// services.AddBaseQueryHandlers(typeof(BaseDto).Assembly);
services.AddScoped<ISeed, Seed>();

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

        // Позволяем использовать токены в SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<NotificationHub>("/hub");
// app.MapHub<ChatHub>("/chatHub");

app.MapControllers();
app.UseHttpsRedirection();

app.MapGet("/", () => "Main messenger page!");

app.Run();

namespace Pilot.Messenger
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program;
}