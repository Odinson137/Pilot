using Pilot.BlazorClient.Components;
using Pilot.BlazorClient.Data;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.Service;
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

services.AddScoped<IGateWayApiService, GateWayApiService>();
services.AddScoped<ICompanyPageService, CompanyPageService>();
services.AddScoped<ICompanyDetailPageService, CompanyDetailPageService>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger());

services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();