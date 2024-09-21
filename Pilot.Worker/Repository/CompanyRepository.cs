using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Worker.Models;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Repository;

public class CompanyRepository(DataContext context, IMapper mapper) : BaseRepository<Models.Company>(context, mapper), ICompany
{
    public async Task<bool> CheckCompanyTitleExistAsync(string title)
    {
        return await DbSet.AnyAsync(c => c.Title == title);
    }
}