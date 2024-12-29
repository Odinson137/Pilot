using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Test.Base.IntegrationBase;
using DataContext = Pilot.Messenger.Data.DataContext;
using Program = Pilot.Messenger.Program;

namespace Test.Api.MessengerService.Factory;

public class MessengerTestMessengerFactory : WebApplicationFactory<Program>
{
    // private readonly RabbitMqContainer _rabbitContainer = new RabbitMqBuilder()
    //     .WithImage("rabbitmq:3")
    //     .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        // Environment.SetEnvironmentVariable("RabbitMQ:ConnectionString",
        //     _rabbitContainer.GetConnectionString());
        
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
        });
    }
}