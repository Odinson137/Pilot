using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Repository;

public class JobApplicationRepository(DataContext context, IMapper mapper)
    : BaseRepository<JobApplication>(context, mapper), IJobApplication
{
    public async Task<Tuple<int, int>?> GetJobApplicationCompanyAndPostIdsAsync(int companyPostId)
    {
        return await DbSet.Where(c => c.CompanyPost.Id == companyPostId)
            .Select(c => new Tuple<int, int>(c.CompanyPost.Post.CompanyId, c.CompanyPost.Post.Id))
            .FirstOrDefaultAsync();
    }
}