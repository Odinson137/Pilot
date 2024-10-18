using Microsoft.AspNetCore.Components.Authorization;
using Pilot.BlazorClient.Components;
using Pilot.BlazorClient.Data;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.Service;
using Pilot.BlazorClient.Service.Pages;
using Pilot.Contracts.Data.Enums;
using Serilog;

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
services.AddScoped<ICompanyDetailPageService, CompanyDetailPageService>();
services.AddScoped<IUserPageService, UserPageService>();
services.AddScoped<IWorkPageService, WorkPageService>();
services.AddScoped<IProjectTaskPageService, ProjectTaskPageService>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger());

services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login"; // URL страницы входа
        options.LogoutPath = "/User/Logout"; // URL для выхода
        options.AccessDeniedPath = "/AccessDenied"; // URL для ошибки доступа
    });

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();