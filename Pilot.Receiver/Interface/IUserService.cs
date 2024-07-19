using Pilot.Contracts.Base;
using Pilot.Contracts.Models;

namespace Pilot.Receiver.Interface;

public interface IUserService : IBaseHttpService
{
    public Task<UserDto?> GetUserByIdAsync(string userId);
}