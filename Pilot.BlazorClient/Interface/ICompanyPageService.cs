using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Interface;

public interface ICompanyPageService
{
    public Task<ICollection<CompanyViewModel>> GetCompanyListAsync();
}