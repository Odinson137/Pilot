using Pilot.Contracts.Base;
using Pilot.Identity.Models;

namespace Pilot.Identity.Interfaces;

public interface IUser : IBaseRepository<UserModel>
{
    public Task<bool> IsUserNameExistAsync(string userName);
    public Task<UserModel?> GetByNameAsync(string userName);
}