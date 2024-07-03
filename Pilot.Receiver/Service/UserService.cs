using Pilot.Contracts.Base;
using Pilot.Identity.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class UserService : BaseHttpService, IUserService
{
    public UserService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory) 
        : base(logger, httpClientFactory, nameof(UserModel))
    {
    }
}