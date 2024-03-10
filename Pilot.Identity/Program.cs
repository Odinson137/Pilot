using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Pilot.Contracts.Services.LogService;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;
using Pilot.Identity.Repository;
using Pilot.Identity.Services;
using Pilot.Receiver.DTO;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var mongoConfiguration = configuration.GetSection("MongoDatabase").Get<MongoConfig>()!;
services.AddSingleton(
    new MongoClient(mongoConfiguration.ConnectionString).GetDatabase(mongoConfiguration.DbName));

services.AddTransient<IToken, TokenService>();
services.AddTransient<IPasswordCoder, PasswordCoderService>();
services.AddScoped<IUser, UserRepository>();

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

var app = builder.Build();

await app.Seeding();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Это сервис для работы с пользователями");

app.MapPost("/Registration", async (
        IUser user, 
        IPasswordCoder passwordService,
        [FromBody] RegistrationUserDto registrationUser) =>
    {

        if (await user.IsUserNameExistAsync(registrationUser.UserName))
        {
            return Results.BadRequest("This username is already taken");
        }
        
        var newUser = new User
        {
            UserName = registrationUser.UserName,
            Name = registrationUser.Name,
            LastName = registrationUser.LastName,
            Password = passwordService.PasswordCode(registrationUser.Password)
        };

        await user.RegistrationAsync(newUser);
        
        return Results.Ok(newUser.Id);
    })
    .WithOpenApi();

app.MapPost("/Authorization", async (
        IUser userRepository, 
        IToken tokenService,
        IPasswordCoder passwordService,
        [FromBody] AuthorizationUserDto userDto) =>
    {

        var user = await userRepository.GetUserAsync(userDto.UserName);

        if (user == null)
        {
            return Results.NotFound("User not found");
        }

        if (user.Password != passwordService.PasswordCode(userDto.Password))
        {
            return Results.BadRequest("Passwords aren't match");
        }

        return Results.Ok(new AuthUserDto(user.Id, tokenService.GenerateToken(user.Id, user.Role)));
    })
    .WithOpenApi();

app.Run();

public record AuthUserDto(string UserId, string Token);

