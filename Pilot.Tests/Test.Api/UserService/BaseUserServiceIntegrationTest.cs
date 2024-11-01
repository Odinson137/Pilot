using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;
using Test.Api.UserService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.UserService;

public class BaseUserServiceIntegrationTest : IClassFixture<ApiTestApiFactory>, IClassFixture<ApiTestIdentityFactory>
{
    private readonly IServiceProvider _identityScopeService;
    protected readonly IToken TokenService;
    protected readonly IPasswordCoder PasswordCoder;
    protected readonly HttpClient ApiClient;
    protected Pilot.Identity.Data.DataContext AssertWorkerContext 
        => _identityScopeService.CreateScope().ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();

    private readonly Dictionary<ServiceName, DbContext> _contexts = new();

    protected DbContext GetContext<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        return _contexts[serviceName];
    }
    
    protected BaseUserServiceIntegrationTest(ApiTestApiFactory apiFactory, ApiTestIdentityFactory identityFactory)
    {
        _identityScopeService = identityFactory.Services.CreateScope().ServiceProvider;
        _contexts[ServiceName.IdentityServer] = _identityScopeService.GetRequiredService<DataContext>();
        
        TokenService = apiFactory.Services.GetRequiredService<IToken>();
        PasswordCoder = _identityScopeService.GetRequiredService<IPasswordCoder>();
        
        var identityClient = identityFactory.CreateClient();
        
        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityClient;

        ApiClient = apiFactory.CreateClient();
    }
}