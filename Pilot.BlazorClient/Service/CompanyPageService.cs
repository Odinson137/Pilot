using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service;

public class CompanyPageService(IGateWayApiService apiService) : ICompanyPageService
{
    public async Task<ICollection<CompanyViewModel>> GetCompanyListAsync()
    {
        var companies = await apiService.SendGetMessages<CompanyDto, CompanyViewModel>();

        foreach (var company in companies)
        {
            var projectList = new List<ProjectViewModel>();

            foreach (var project in company.Projects)
            {
                var projectViewModel = await apiService.SendGetMessage<ProjectDto, ProjectViewModel>(project.Id);
                projectList.Add(projectViewModel);
            }

            company.Projects = projectList;
        }
        
        return companies;
    }
}