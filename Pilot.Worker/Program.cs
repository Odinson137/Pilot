using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;
using Pilot.Worker.Repository;
using Pilot.Worker.Service;
using Pilot.SqrsControllerLibrary;
using Pilot.SqrsControllerLibrary.Behaviors;
using Pilot.SqrsControllerLibrary.Interfaces;
using Pilot.SqrsControllerLibrary.NotificationHandlers;
using Pilot.SqrsControllerLibrary.Notifications;
using Pilot.SqrsControllerLibrary.Services;
using Pilot.Worker.Behaviors;

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
services.AddScoped<ITeamEmployee, TeamEmployeeRepository>();

services.AddScoped<IBaseValidatorService, ValidatorService>();

services.AddScoped<IBaseMassTransitService, BaseMassTransitService>();
services.AddScoped<IMessageService, MessageService>();

services.AddScoped<IModelService, ModelService>();

builder.AddBaseServices<DataContext, AutoMapperProfile, Program>();
builder.AddUnitOfWork<UnitOfWork>();

services.AddHttpClient(ServiceName.IdentityServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("IdentityServerUrl")!); });

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddControllers();

services.AddScoped<ISeed, Seed>();

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AddCompanyUserBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));

services.AddScoped<INotificationHandler<MessageSentNotification>, MessageSentNotificationHandler>();

services.AddRedis(configuration);

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


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Main receiver page!");

app.MapControllers();
app.UseHttpsRedirection();

app.Run();

namespace Pilot.Worker
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class Program;
}