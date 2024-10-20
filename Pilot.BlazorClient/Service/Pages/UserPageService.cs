using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;

namespace Pilot.BlazorClient.Service.Pages;

public class UserPageService(IGateWayApiService apiService, IUserService userService, ILogger<UserPageService> logger, IMapper mapper) : IUserPageService
{
    public async Task Registration(RegistrationUserViewModel registrationUser)
    {
        var client = await apiService.GetClientAsync<AuthorizationUserViewModel>();
        var response = await client.PostAsJsonAsync("api/User/Registration", registrationUser);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
    
    public async Task<AuthUserViewModel> Authorization(AuthorizationUserViewModel authorizationUser)
    {
        var client = await apiService.GetClientAsync<AuthorizationUserViewModel>();
        var response = await client.PostAsJsonAsync("api/User/Authorization", authorizationUser);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var auth = await response.Content.ReadFromJsonAsync<AuthUserViewModel>();

        if (auth == null)
        {
            throw new Exception("Ошибка конвертации файла");
        }
        
        return auth;
    }
    
    public async Task<UserViewModel> GetUserAsync()
    {
        var user = await userService.GetCurrentUserAsync();
        return user;
    }

    public async Task<UserViewModel> GetAnotherUserAsync(int userId)
    {
        var userDto = await apiService.SendGetMessage<UserDto>(userId.ToString());

        var user = userDto.Map<UserViewModel>(mapper);
        return user;
    }
}