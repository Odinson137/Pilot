using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Services;
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

services.AddTransient<IToken, TokenService>();
services.AddTransient<IPasswordCoder, PasswordCoderService>();
services.AddScoped<IUser, UserRepository>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    // .WriteTo.Logger(lc =>
    // {
    //     lc.MinimumLevel.Error();
    //     lc.WriteTo.MongoDb(configuration.GetSection("MongoDatabase").Get<MongoConfig>()!);
    // })
    .CreateLogger());

services.AddTransient<ISeed, Seed>();

services.AddAutoMapper(typeof(AutoMapperProfile));

services.AddControllers();

services.AddDbContext<DataContext>(option => option.UseMySql(
        configuration.GetConnection("MySql:ConnectionString"),
        new MySqlServerVersion(new Version(8, 0, 11))
    )
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

var app = builder.Build();

// await app.Services.GetRequiredService<ISeed>().Seeding(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
        
        var newUser = new UserModel
        {
            UserName = registrationUser.UserName,
            Name = registrationUser.Name,
            LastName = registrationUser.LastName,
            Password = passwordService.PasswordCode(registrationUser.Password)
        };

        await user.AddNewValueAsync(newUser);

        await user.SaveAsync();
        
        logger.LogInformation("Received registration form in identity");

        return Results.Ok();
    })
    .WithOpenApi();

app.MapPost("/Authorization", async (
        IUser userRepository, 
        IToken tokenService,
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

        if (user.Password != passwordService.PasswordCode(userDto.Password))
        {
            logger.LogInformation("Passwords aren't match");
            return Results.BadRequest("Passwords aren't match");
        }

        logger.LogInformation("Received authorization form in identity");
        return Results.Ok(new AuthUserDto(user.Id, tokenService.GenerateToken(user.Id, user.Role)));
    })
    .WithOpenApi();

app.MapPut("/Change", async (
        IUser userRepository, 
        ILogger<Program> logger,
        IPasswordCoder passwordService,
        [FromBody] UpdateUserDto userDto) =>
    {
        logger.LogInformation("Receive change form in identity");

        var user = await userRepository.GetByIdAsync(userDto.Id);

        if (user == null)
        {
            logger.LogInformation("User not found");
            return Results.NotFound("User not found");
        }

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            logger.LogInformation("Passwords change");
            
            if (user.Password != passwordService.PasswordCode(userDto.OldPassword))
            {
                logger.LogInformation("Passwords aren't match");
                return Results.BadRequest("Passwords aren't match");
            }

            user.Password = passwordService.PasswordCode(userDto.Password);
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
    public partial class Program {}
}
