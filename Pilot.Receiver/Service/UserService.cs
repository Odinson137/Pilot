using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Contracts.Services.LogService;
using Pilot.Identity.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class UserService : BaseHttpService, IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IDistributedCache _cache;
    
    public UserService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory, IDistributedCache cache) 
        : base(logger, httpClientFactory, nameof(UserModel))
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        _logger.LogInformation($"Getting user by id - {userId}");
        
        var cacheUser = await _cache.GetStringAsync($"User-{userId}");
        
        UserDto? userDto;
        if (string.IsNullOrEmpty(cacheUser))
        {
            _logger.LogInformation("Get user from cache");
            userDto = await SendGetOneMessage<UserDto>($"api/User/{userId}", default);
        }
        else
        {
            _logger.LogInformation("Get user from db");
            userDto = JsonConvert.DeserializeObject<UserDto>(cacheUser);
        }
        
        _logger.LogClassInfo(userDto);
        return userDto;
    }
}