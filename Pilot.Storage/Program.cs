using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary;
using Pilot.Storage.Data;
using Pilot.Storage.Interface;
using Pilot.Storage.Repository;
using Pilot.Storage.Service;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IStorageService, GoogleStorageService>();
services.AddScoped<IFileRepository, FileRepository>();
services.AddScoped<IFileService, FileService>();

services.AddScoped<IBaseValidatorService, StorageValidateService>();
services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();
services.AddScoped<IMessageService, MessageService>();

builder.AddBaseServices<DataContext, AutoMapperProfile, Program>();

services.AddControllers();

services.AddScoped<ISeed, Seed>();

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeed>().Seeding();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Storage service!");

app.Run();

namespace Pilot.Storage
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program;
}