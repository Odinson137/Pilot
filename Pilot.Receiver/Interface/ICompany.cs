using Pilot.Contracts.DTO;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Interface;

public interface ICompany
{
    Task<ICollection<CompanyDto>> GetCompaniesAsync(CancellationToken cancellationToken);
    Task<CompanyDto?> GetCompanyAsync(string id, CancellationToken cancellationToken);
    public Task<Company?> CheckCompanyTitleExistAsync(string title);
    public Task AddCompanyAsync(Company company);
    public Task ChangeCompanyTitleAsync(string companyId, string companyTitle);
}