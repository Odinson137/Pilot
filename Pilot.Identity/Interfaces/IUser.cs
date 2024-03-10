using Pilot.Identity.Models;

namespace Pilot.Identity.Interfaces;

public interface IUser
{
    public Task RegistrationAsync(User user);
    public Task<bool> IsUserNameExistAsync(string userName);
    public Task<User?> GetUserAsync(string userId);
}