using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services;
using Pilot.Messenger.Interface;
using Pilot.Messenger.Repository;
using Pilot.Messenger.Services;
using Pilot.SqrsController.Behaviors;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IBaseValidatorService, ValidatorService>();
services.AddUserService();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
