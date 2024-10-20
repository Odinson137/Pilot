using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class CompanyPageService(IGateWayApiService apiService) : ICompanyPageService
{
    public async Task<ICollection<CompanyViewModel>> GetCompanyListAsync()
    {
        var companies = await apiService.SendGetMessages<CompanyDto, CompanyViewModel>();
        
        return companies;
    }
}