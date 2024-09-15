using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service;

public class CompanyPageService(IGateWayApiService apiService) : ICompanyPageService
{
    public async Task<ICollection<CompanyViewModel>> GetCompanyListAsync()
    {
        var companies = await apiService.SendGetMessages<CompanyDto, CompanyViewModel>();

        foreach (var company in companies)
        {
            var idArray = company.Projects.Select(c => c.Id).ToArray();
            var projectViewModels =
                await apiService.SendGetMessages<ProjectDto, ProjectViewModel>(
                    filter: new BaseFilter(idArray));
            company.Projects = projectViewModels;
        }
        
        return companies;
    }
}