using Pilot.Contracts.Models;

namespace Pilot.Receiver.Interface;

public interface ICompany
{
    public Task<Company?> CheckCompanyTitleExistAsync(string title);
    public Task AddCompanyAsync(Company company);
    public Task ChangeCompanyTitleAsync(string companyId, string companyTitle);
}