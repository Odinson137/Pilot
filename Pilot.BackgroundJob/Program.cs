using Hangfire;
using Pilot.BackgroundJob.Data;
using Pilot.Contracts.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
services.AddControllers();

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});

services.AddScoped<ISeed, Seed>();

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

app.MapGet("/", () => "This is Pilot.BackgroundJob service");

app.Run();

namespace Pilot.BackgroundJob
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program;
}