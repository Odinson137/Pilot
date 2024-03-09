using Pilot.Contracts.Models;

namespace Pilot.Receiver.Interface;

public interface IUser
{
    public Task<User?> GetUserByIdAsync(string userId);
}