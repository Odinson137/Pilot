﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry.Trace;
using Pilot.Contracts.Data;
using Test.Base.IntegrationBase;
using DataContext = Pilot.Identity.Data.DataContext;

namespace Test.BackgroundJob.Factories;

public class BackgroundJobTestIdentityFactory : WebApplicationFactory<Pilot.Identity.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISeed>(); // must remove if you don't to call the seed code in your tests
            services.AddTransient<ISeed, TestSeed>();

            services.RemoveAll<DbContextOptions<DataContext>>();
            services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("TestDatabase"); });
            
            services.RemoveAll<TracerProvider>();
            services.AddSingleton(TracerProvider.Default);
        });
    }
}