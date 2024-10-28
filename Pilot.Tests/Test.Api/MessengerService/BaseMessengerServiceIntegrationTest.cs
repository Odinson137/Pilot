using System.Net.Http.Headers;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Messenger.Data;
using Test.Api.MessengerService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.MessengerService;

public class BaseMessengerServiceIntegrationTest : 
    IClassFixture<MessengerTestApiFactory>, 
    IClassFixture<MessengerTestIdentityFactory>,
    IClassFixture<MessengerTestMessengerFactory>
{
    private readonly IServiceProvider _scopeService;
    protected readonly HttpClient ApiClient;
    protected readonly IMapper Mapper;

    private readonly Dictionary<ServiceName, DbContext> _contexts = new();
    
    protected DataContext AssertContext 
        => _scopeService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    protected DbContext GetContext<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        return _contexts[serviceName];
    }

    protected BaseMessengerServiceIntegrationTest(MessengerTestApiFactory apiFactory, MessengerTestIdentityFactory identityFactory, MessengerTestMessengerFactory messengerFactory)
    {
        ApiClient = apiFactory.CreateClient();

        Mapper = messengerFactory.Services.GetRequiredService<IMapper>();
        var tokenService = apiFactory.Services.GetRequiredService<IToken>();
        var token = tokenService.GenerateToken(1, Role.User);
        
        ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var identityScopeService = identityFactory.Services.CreateScope().ServiceProvider;
        _scopeService = messengerFactory.Services.CreateScope().ServiceProvider;

        _contexts[ServiceName.IdentityServer] = identityScopeService.GetRequiredService<Pilot.Identity.Data.DataContext>();
        _contexts[ServiceName.MessengerServer] = _scopeService.GetRequiredService<Pilot.Messenger.Data.DataContext>();
        
        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.MessengerServer.ToString()] = messengerFactory.CreateClient() ;
    }
}