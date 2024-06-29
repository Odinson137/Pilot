using Pilot.Contracts.Base;
using Pilot.Identity.Models;

namespace Pilot.Identity.Interfaces;

public interface IUser : IBaseRepository<User>
{
    public Task<bool> IsUserNameExistAsync(string userName);
    public Task<User?> GetByNameAsync(string userName);
}