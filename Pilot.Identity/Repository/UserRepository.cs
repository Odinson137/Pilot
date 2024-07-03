using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;

namespace Pilot.Identity.Repository;

public class UserRepository(DataContext context, IMapper mapper) : BaseRepository<UserModel>(context, mapper), IUser
{
    public async Task<bool> IsUserNameExistAsync(string userName)
    {
        return await DbSet.AnyAsync(c => c.Name == userName);
    }

    public async Task<UserModel?> GetByNameAsync(string userName)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.UserName == userName);
    }
}