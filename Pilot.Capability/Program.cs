using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Repository;
using Pilot.Capability.Service;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary;
using Pilot.SqrsControllerLibrary.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
services.AddControllers();

builder.AddBaseServices<DataContext, AutoMapperProfile, Program>();

services.AddScoped<IPost, PostRepository>();
services.AddScoped<ICompanyPost, CompanyPostRepository>();
services.AddScoped<ISkill, SkillRepository>();
services.AddScoped<IUserSkill, UserSkillRepository>();

services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IValidatorService, ValidatorService>();
services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

services.AddScoped<ISeed, Seed>();


var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

app.MapGet("/", () => "This is capability service");

app.Run();

namespace Pilot.Capability
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program;
}