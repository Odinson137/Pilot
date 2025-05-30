using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Identity.Data;
using Pilot.Identity.DTO;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;
using Pilot.Identity.Repository;
using Pilot.Identity.Services;
using Pilot.SqrsControllerLibrary;
using Pilot.SqrsControllerLibrary.Behaviors;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddTransient<IPasswordCoder, PasswordCoderService>();
services.AddScoped<IUser, UserRepository>();

services.AddScoped<ISeed, Seed>();

services.AddControllers();

services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

services.AddScoped<IBaseValidatorService, ValidatorService>();

builder.AddBaseServices<DataContext, AutoMapperProfile, Program>();
builder.AddUnitOfWork<UnitOfWork>();
services.AddRedis(configuration);

services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

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

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

app.MapGet("/", () => "Это сервис для работы с пользователями");

app.MapPost("/Registration", async (
        IUser user,
        IPasswordCoder passwordService,
        ILogger<Program> logger,
        [FromBody] RegistrationUserDto registrationUser) =>
    {
        logger.LogInformation("Receive registration form in identity");

        if (await user.IsUserNameExistAsync(registrationUser.UserName))
        {
            logger.LogInformation("This username is already taken");
            return Results.BadRequest("This username is already taken");
        }

        var items = passwordService.GenerateSaltAndHashPassword(registrationUser.Password);
        
        var newUser = new User
        {
            UserName = registrationUser.UserName,
            Name = registrationUser.Name,
            LastName = registrationUser.LastName,
            Password = items.Item1,
            Salt = items.Item2
        };

        await user.AddValueToContextAsync(newUser);

        await user.SaveAsync();
        
        logger.LogInformation("Received registration form in identity");

        return Results.Ok();
    })
    .WithOpenApi();

app.MapPost("/Authorization", async (
        IUser userRepository,
        ILogger<Program> logger,
        IPasswordCoder passwordService,
        [FromBody] AuthorizationUserDto userDto) =>
    {
        logger.LogInformation("Receive authorization form in identity");

        var user = await userRepository.GetByNameAsync(userDto.UserName);

        if (user == null)
        {
            logger.LogInformation("User not found");
            return Results.NotFound("User not found");
        }

        // TODO для тестов, не забыдь потом включить
        // var realPassword = passwordService.ComparePasswordAndSalt(userDto.Password, user.Salt);
        // if (user.Password != realPassword)
        // {
        //     logger.LogInformation("Passwords aren't match");
        //     return Results.BadRequest("Passwords aren't match");
        // }

        logger.LogInformation("Received authorization form in identity");
        return Results.Ok(new AuthUserRoleDto(user.Id, user.Role));
    })
    .WithOpenApi();


// TODO потом перенести в контроллер дабы использовать Authorize
app.MapPut("/Change", async (
        IUser userRepository,
        ILogger<Program> logger,
        [FromBody] UpdateUserDto userDto) =>
    {
        logger.LogInformation("Receive change form in identity");

        var user = await userRepository.GetByIdAsync(userDto.Id);

        if (user == null)
        {
            logger.LogInformation("User not found");
            return Results.NotFound("User not found");
        }

        user.Name = user.Name;
        user.LastName = user.LastName;

        logger.LogInformation("Received change user form in identity");
        return Results.Ok();
    })
    .WithOpenApi();

app.Run();

namespace Pilot.Identity
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program
    {
    }
}