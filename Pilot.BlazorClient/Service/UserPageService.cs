using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Service;

public class UserPageService(IGateWayApiService apiService, ILogger<UserPageService> logger) : IUserPageService
{
    public async Task Registration(RegistrationUserViewModel registrationUser)
    {
        var response = await apiService.GetClient<RegistrationUserViewModel>().PostAsJsonAsync("api/Registration", registrationUser);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
    
    public async Task<AuthUserViewModel> Authorization(AuthorizationUserViewModel authorizationUser)
    {
        var response = await apiService.GetClient<AuthorizationUserViewModel>().PostAsJsonAsync("api/Authorization", authorizationUser);

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
}