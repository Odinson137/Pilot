using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Service.Pages;

public class SummaryPageService(
    IBaseModelService<ProjectTaskViewModel> projectTaskService,
    IBaseModelService<TeamViewModel> teamTaskService 
) : ISummaryPageService
{
    public async Task<ICollection<ProjectTaskViewModel>> GetUserProjectTasksAsync(int userId)
    {
        var result = 
            await projectTaskService.GetValuesAsync(c => c.TeamEmployee!.CompanyUser!.Id, userId);
        return result;
    }

    public async Task<ICollection<ProjectTaskViewModel>> GetCompanyTasksAsync(int companyId)
    {
        var result = 
            await projectTaskService.GetValuesAsync(c => c.TeamEmployee!.CompanyUser!.Company.Id, companyId);
        return result;
    }

    public async Task<ICollection<ProjectTaskViewModel>> FillProjectsIntoTeamsAsync(ICollection<ProjectTaskViewModel> projectTasks)
    {
        var ids = projectTasks.Select(c => c.TeamEmployee!.Team.Id).ToList();
        var result = await teamTaskService.GetValuesAsync(ids);
        foreach (var projectTask in projectTasks)
            projectTask.TeamEmployee!.Team = result.First(c => c.Id == projectTask.TeamEmployee!.Team.Id);
        return projectTasks;
    }
}