using Pilot.BlazorClient.Components;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.Service;
using Pilot.Contracts.Data.Enums;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddHttpClient(ServiceName.ApiServer.ToString(),
    c => { c.BaseAddress = new Uri(configuration.GetValue<string>("ApiServerUrl")!); });

services.AddScoped<IGateWayApiService, GateWayApiService>();

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