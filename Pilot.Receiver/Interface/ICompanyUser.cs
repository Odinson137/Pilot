using Pilot.Contracts.DTO;

namespace Pilot.Receiver.Interface;

public interface ICompanyUser
{
    public Task<ICollection<CompanyUserDto>> GetCompanyUsersAsync(string companyId);
    public Task<CompanyUserDto> GetCompanyUserAsync(string companyId, string userId);
}