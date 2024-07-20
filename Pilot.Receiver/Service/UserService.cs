using Microsoft.Extensions.Caching.Distributed;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class UserService : ModelService<UserDto>, IUserService
{
    public UserService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory, IDistributedCache cache) 
        : base(logger, httpClientFactory, cache, "IdentityServer")
    {
    }
}