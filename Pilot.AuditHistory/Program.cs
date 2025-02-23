using ClickHouse.Client.ADO.Parameters;
using ClickHouse.EntityFrameworkCore.Extensions;
using Pilot.AuditHistory.Data;
using Pilot.AuditHistory.Interface;
using Pilot.AuditHistory.Models;
using Pilot.AuditHistory.Repository;
using Pilot.Contracts.Data;
using Pilot.AuditHistory.Service;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
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

builder.AddBaseServices<AutoMapperProfile, Program>();

builder.Services.AddEntityFrameworkClickHouse();
builder.Services.AddDbContext<ClickHouseContext>(options =>
{
    var connectionString = builder.Configuration["ClickHouse:ConnectionString"];
    options.UseClickHouse(connectionString);
});

#region Repository realization

// services.AddScoped<IAuditHistory, AuditHistoryRepository>();
services.AddScoped<IClickHouseService, ClickHouseService>();

#endregion

#region Service realization

services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IBaseValidatorService, ValidatorService>();
services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

#endregion

services.AddScoped<ISeed, Seed>();

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

app.MapGet("/", () => "This is Pilot.AuditHistory service");

app.Run();

namespace Pilot.AuditHistory
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once UnusedType.Global
    public partial class Program;
}