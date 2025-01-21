using Blazored.Modal;
using Blazored.Toast;
using BlazorStrap;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Pilot.BlazorClient.Components;
using Pilot.BlazorClient.Data;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.Service;
using Pilot.BlazorClient.Service.Pages;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Serilog;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddHttpClient(ServiceName.ApiServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("ApiServerUrl")!); });

services.AddScoped<IUserService, UserService>();

services.AddScoped<IGateWayApiService, GateWayApiService>();
services.AddScoped<ICompanyPageService, CompanyPageService>();
services.AddScoped<IUserPageService, UserPageService>();
services.AddScoped<IWorkPageService, WorkPageService>();
services.AddScoped<IProjectTaskPageService, ProjectTaskPageService>();
services.AddScoped<IChatPageService, ChatPageService>();
services.AddScoped<IMessengerService, MessengerService>();
services.AddScoped<IReminderPageService, ReminderPageService>();
services.AddScoped<ICompanyPostPageService, CompanyPostPageService>();

// Base model service registrations
services.AddScoped<IBaseModelService<ProjectViewModel>, BaseModelService<ProjectDto, ProjectViewModel>>();
services.AddScoped<IBaseModelService<TaskInfoViewModel>, BaseModelService<TaskInfoDto, TaskInfoViewModel>>();
services.AddScoped<IBaseModelService<ChatViewModel>, BaseModelService<ChatDto, ChatViewModel>>();
services.AddScoped<IBaseModelService<ChatMemberViewModel>, BaseModelService<ChatMemberDto, ChatMemberViewModel>>();
services.AddScoped<IBaseModelService<ChatReminderViewModel>, BaseModelService<ChatReminderDto, ChatReminderViewModel>>();
services.AddScoped<IBaseModelService<CompanyViewModel>, BaseModelService<CompanyDto, CompanyViewModel>>();
services.AddScoped<IBaseModelService<CompanyPostViewModel>, BaseModelService<CompanyPostDto, CompanyPostViewModel>>();
services.AddScoped<IBaseModelService<CompanyRoleViewModel>, BaseModelService<CompanyRoleDto, CompanyRoleViewModel>>();
services.AddScoped<IBaseModelService<CompanyUserViewModel>, BaseModelService<CompanyUserDto, CompanyUserViewModel>>();
services.AddScoped<IBaseModelService<HistoryActionViewModel>, BaseModelService<HistoryActionDto, HistoryActionViewModel>>();
services.AddScoped<IBaseModelService<InfoMessageViewModel>, BaseModelService<InfoMessageDto, InfoMessageViewModel>>();
services.AddScoped<IBaseModelService<JobApplicationViewModel>, BaseModelService<JobApplicationDto, JobApplicationViewModel>>();
services.AddScoped<IBaseModelService<MessageViewModel>, BaseModelService<MessageDto, MessageViewModel>>();
services.AddScoped<IBaseModelService<PostViewModel>, BaseModelService<PostDto, PostViewModel>>();
services.AddScoped<IBaseModelService<ProjectTaskViewModel>, BaseModelService<ProjectTaskDto, ProjectTaskViewModel>>();
services.AddScoped<IBaseModelService<SkillViewModel>, BaseModelService<SkillDto, SkillViewModel>>();
services.AddScoped<IBaseModelService<TeamViewModel>, BaseModelService<TeamDto, TeamViewModel>>();
services.AddScoped<IBaseModelService<UserSkillViewModel>, BaseModelService<UserSkillDto, UserSkillViewModel>>();

services.AddSingleton<IJsonLocalizationService, JsonLocalizationService>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger());