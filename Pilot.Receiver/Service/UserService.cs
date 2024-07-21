using Microsoft.Extensions.Caching.Distributed;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class UserService : ModelService<UserDto>, IUserService
{
    public UserService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory, IDistributedCache cache, IConfiguration configuration) 
        : base(logger, httpClientFactory, cache, configuration,"IdentityServer")
    {
    }
}