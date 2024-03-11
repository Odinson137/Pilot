using MongoDB.Bson;
using Pilot.Api.DTO;
using Pilot.Api.Interfaces.Interfaces;
using Pilot.Receiver.DTO;

namespace Pilot.Api.Services;

public class UserService : IUser
{
    private const string Url = " https://localhost:7127/";
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task RegistrationAsync(RegistrationUserDto userDto, CancellationToken token)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Url}/Registration", userDto, token);
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.Content.ToString());
    }

    public async Task<AuthUserDto> AuthorizationAsync(AuthorizationUserDto userDto, CancellationToken token)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Url}/Authorization", userDto, token);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.Content.ToString());
        }

        var content = await response.Content.ReadFromJsonAsync<AuthUserDto>(token);
        return content!;
    }
}

