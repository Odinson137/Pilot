using Blazored.Modal;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Pilot.BlazorClient.Components;
using Pilot.BlazorClient.Data;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.Service;
using Pilot.BlazorClient.Service.Pages;
using Pilot.Contracts.Data.Enums;
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
services.AddSingleton<IJsonLocalizationService, JsonLocalizationService>();

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