using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IUserPageService
{
    public Task Registration(RegistrationUserViewModel registrationUser);

    public Task<AuthUserViewModel> Authorization(AuthorizationUserViewModel authorizationUser);
}