using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Razor;
using Pilot.BlazorClient.Components;
using Pilot.BlazorClient.Data;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.Service;
using Pilot.BlazorClient.Service.Pages;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
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
services.AddScoped<IBaseModelService<UserViewModel>, BaseModelService<UserDto, UserViewModel>>();
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
services.AddScoped<IBaseModelService<FileViewModel>, BaseModelService<FileDto, FileViewModel>>();

services.AddSingleton<IJsonLocalizationService, JsonLocalizationService>();

builder.Services.AddScoped<IErrorBoundaryLogger, GlobalErrorHandler>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger());

services.AddAutoMapper(typeof(AutoMapperProfile));

services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; // URL страницы входа
        options.LogoutPath = "/User/Logout"; // URL для выхода
        options.AccessDeniedPath = "/AccessDenied"; // URL для ошибки доступа
    });

services.AddAuthorizationCore();
services.AddCascadingAuthenticationState();
services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
services.AddScoped<TokenAuthenticationStateProvider>();

services.AddBlazoredModal();
services.AddSyncfusionBlazor();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddBlazoredToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseRequestLocalization(new RequestLocalizationOptions()
    .AddSupportedCultures(new[] { "en-US", "ru-RU" })
    .AddSupportedUICultures(new[] { "en-US", "ru-RU" }));

app.UseHttpsRedirection();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();