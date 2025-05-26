using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Repository;

public class CompanyRepository(DataContext context, ReadOnlyDataContext readOnlyDataContext, IMapper mapper) : BaseReadWriteSplitRepository<Models.Company>(context, readOnlyDataContext, mapper), ICompany
{
    public async Task<bool> CheckCompanyTitleExistAsync(string title)
    {
        return await DbSet.AnyAsync(c => c.Title == title);
    }
}