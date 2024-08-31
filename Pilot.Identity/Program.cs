using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO;
using Pilot.Identity.Data;
using Pilot.Identity.DTO;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;
using Pilot.Identity.Repository;
using Pilot.Identity.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddTransient<IPasswordCoder, PasswordCoderService>();
services.AddScoped<IUser, UserRepository>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger());

services.AddScoped<ISeed, Seed>();

services.AddAutoMapper(typeof(AutoMapperProfile));

services.AddControllers();

services.AddDbContext<DataContext>(option => option.UseMySql(
        configuration["MySql:ConnectionString"],
        new MySqlServerVersion(new Version(8, 0, 11))
    )
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

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

        var realPassword = passwordService.ComparePasswordAndSalt(userDto.Password, user.Salt);
        if (user.Password != realPassword)
        {
            logger.LogInformation("Passwords aren't match");
            return Results.BadRequest("Passwords aren't match");
        }

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
    public class Program
    {
    }
}