using Pilot.Capability.Data;
using Pilot.Contracts.Data;
using Pilot.SqrsControllerLibrary;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddBaseServices<DataContext, Program, AutoMapperProfile>();

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/", () => "This is skill service");

app.Run();

namespace Pilot.Capability
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program;
}