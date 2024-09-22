using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Contracts.Interfaces;
using Pilot.InvalidationCacheRedisLibrary;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;
using Pilot.Worker.Repository;
using Pilot.Worker.Service;
using Pilot.SqrsControllerLibrary;
using Pilot.SqrsControllerLibrary.Behaviors;
using IBaseValidatorService = Pilot.Contracts.Base.IBaseValidatorService;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddScoped<ICompany, CompanyRepository>();
services.AddScoped<ICompanyUser, CompanyUserRepository>();
services.AddScoped<IHistoryAction, HistoryActionRepository>();
services.AddScoped<IProject, ProjectRepository>();
services.AddScoped<IProjectTask, ProjectTaskRepository>();
services.AddScoped<ITeam, TeamRepository>();
services.AddScoped<ICompanyRole, CompanyRoleRepository>();
services.AddScoped<ITaskInfo, TaskInfoRepository>();

services.AddScoped<IBaseValidatorService, ValidatorService>();

services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();
services.AddScoped<IMessageService, MessageService>();

builder.AddBaseServices<DataContext, AutoMapperProfile, Program>();

services.AddHttpClient(ServiceName.IdentityServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!); });

services.AddHttpClient(ServiceName.StorageServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("StorageServerUrl")!); });

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddControllers();

services.AddScoped<ISeed, Seed>();

// services.AddBaseQueryHandlers(typeof(BaseDto).Assembly);
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

await services.AddRedis(configuration);

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
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.MapGet("/", () => "Main receiver page!");

app.MapControllers();
app.UseHttpsRedirection();

app.Run();

namespace Pilot.Worker
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program;
}