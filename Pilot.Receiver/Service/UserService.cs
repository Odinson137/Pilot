using Microsoft.Extensions.Caching.Distributed;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Receiver.Service;

public class UserService(
    ILogger<UserService> logger,
    IHttpClientFactory httpClientFactory,
    IDistributedCache cache,
    IConfiguration configuration)
    : ModelService<UserDto>(logger, httpClientFactory, cache, configuration, "IdentityServer"), IUserService;