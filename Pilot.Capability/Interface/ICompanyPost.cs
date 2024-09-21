using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Capability.Interface;

public interface ICompanyPost : IBaseRepository<CompanyPost>
{
    public Task<ICollection<CompanyPostDto>> GetOpenPostsAsync(BaseFilter filter, CancellationToken token);

}