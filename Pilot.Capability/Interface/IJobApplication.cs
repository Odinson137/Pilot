using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Interface;

public interface IJobApplication : IBaseRepository<JobApplication>
{
    Task<Tuple<int, int>?> GetJobApplicationCompanyAndPostIdsAsync(int companyPostId);
}