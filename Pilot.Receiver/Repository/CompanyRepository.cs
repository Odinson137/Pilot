using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class CompanyRepository(DbContext context, MapperConfiguration configuration) : BaseRepository<Company>(context, configuration), ICompany
{
    public async Task<bool> CheckCompanyTitleExistAsync(string title)
    {
        return await DbSet.AnyAsync(c => c.Title == title);
    }
}