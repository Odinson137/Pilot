using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IUserService
{
    public Task<UserViewModel> GetCurrentUserAsync();

    Task<bool> IsUserAuthorizationAsync();
}