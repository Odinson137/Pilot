using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;

namespace Pilot.Identity.Repository;

public class UserRepository(DbContext context, MapperConfiguration configuration) : BaseRepository<User>(context, configuration), IUser
{
    public async Task<bool> IsUserNameExistAsync(string userName)
    {
        return await DbSet.AnyAsync(c => c.Name == userName);
    }

    public async Task<User?> GetByNameAsync(string userName)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.UserName == userName);
    }
}