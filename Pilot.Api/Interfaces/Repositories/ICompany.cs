using Pilot.Api.DTO;

namespace Pilot.Api.Interfaces.Repositories;

public interface ICompany
{
    Task<ICollection<CompanyDto>> GetCompaniesAsync(CancellationToken cancellationToken);
    Task<CompanyDto> GetCompanyAsync(string id, CancellationToken cancellationToken);
}