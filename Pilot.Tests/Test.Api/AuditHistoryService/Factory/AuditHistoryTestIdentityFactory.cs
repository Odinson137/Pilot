﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Trace;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Identity;
using Pilot.Identity.Data;
using Test.Base.IntegrationBase;

namespace Test.Api.AuditHistoryService.Factory;

// TODO сделать потом хранилище сервисов для теста, чтоб легко добавлять новые сервисы к тестам без дублирования
public class AuditHistoryTestIdentityFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();
            
            services.RemoveAll<DbContextOptions<DataContext>>();
            
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });
            
            services.RemoveAll<IBaseHttpService>(); 
            services.AddScoped<IBaseHttpService, BaseHttpServiceFaker>();
            
            services.RemoveAll<TracerProvider>();
            services.AddSingleton(TracerProvider.Default);
        });
    }
}