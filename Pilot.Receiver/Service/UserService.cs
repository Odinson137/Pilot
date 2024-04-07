using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.DTO;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class UserService : IUserService
{
    private readonly IDistributedCache _redis;
    private readonly ILogger<UserService> _logger;
    private readonly HttpClient _httpClient;
    public UserService(IDistributedCache redis, ILogger<UserService> logger, IHttpClientFactory httpClientFactory)
    {
        _redis = redis;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("IdentityServer");
    }
    
    public async Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Get user by id {userId}");

        var stringUser = await _redis.GetStringAsync($"session-user-by-id:{userId}", cancellationToken);
        if (!string.IsNullOrEmpty(stringUser))
        {
            _logger.LogInformation("User is from cache");
            var cacheUser = JsonConvert.DeserializeObject<UserDto>(stringUser)!;
            _logger.LogClassInfo(cacheUser);
            return cacheUser;
        }
        
        _logger.LogInformation("User is from Identity server");
        
        var response = await _httpClient.GetAsync("User", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(cancellationToken));   
        }

        var user = await response.Content.ReadFromJsonAsync<UserDto>(cancellationToken);
        _logger.LogClassInfo(user);
        return user!;
    }
}