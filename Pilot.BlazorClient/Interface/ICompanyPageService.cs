using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface ICompanyPageService
{
    public Task<ICollection<CompanyViewModel>> GetCompanyListAsync();
}