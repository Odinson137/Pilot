using Hangfire;
using Pilot.BackgroundJob.Data;
using Pilot.BackgroundJob.Interface;
using Pilot.BackgroundJob.Models;
using Pilot.BackgroundJob.Repository;
using Pilot.BackgroundJob.Service;
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

#region Repository realization

services.AddScoped<IChatReminder, ChatReminderRepository>();
services.AddScoped<IJob, HangfireIJob>();

#endregion

#region Service realization

services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IValidatorService, ValidatorService>();
services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();

#endregion

services.AddHangfire(config =>
{
    config.UseSqlServerStorage(configuration["HangfireConnection:ConnectionString"]);
});

services.AddHangfireServer();

services.AddScoped<ISeed, Seed>();

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseHangfireDashboard();

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "This is Pilot.BackgroundJob service");

app.Run();

namespace Pilot.BackgroundJob
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once UnusedType.Global
    public partial class Program;
}