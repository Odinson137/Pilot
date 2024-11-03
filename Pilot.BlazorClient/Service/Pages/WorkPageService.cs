using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class WorkPageService(IGateWayApiService apiService) : IWorkPageService
{
    public async Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId)
    {
        var companyUserViewModel = await apiService.SendGetMessage<CompanyUserDto, CompanyUserViewModel>(userCompanyId);
        return companyUserViewModel;
    }

    public async Task<ICollection<ProjectViewModel>> GetUserProjectsAsync<T>(T userProjects) where T : ICollection<BaseViewModel>
    {
        var filter = new BaseFilter(userProjects.Select(c => c.Id).ToArray());
        var projectViewModels = await apiService.SendGetMessages<ProjectDto, ProjectViewModel>(filter: filter);
        return projectViewModels;
    }

    public async Task<ICollection<ProjectViewModel>> GetUserProjectsAsync(ICollection<TeamViewModel> userProjects)
    {
        var filter = new BaseFilter(userProjects.Select(c => c.Id).Distinct().ToList());
        var projectViewModels = await apiService.SendGetMessages<ProjectDto, ProjectViewModel>(filter: filter);
        return projectViewModels;
    }

    public async Task<ICollection<TeamViewModel>> GetUserTeamsAsync(ICollection<int> userProjects)
    {
        var filter = new BaseFilter(userProjects.Distinct().ToArray());
        var teamViewModels = await apiService.SendGetMessages<TeamDto, TeamViewModel>(filter: filter);
        return teamViewModels;
    }

    public async Task<ICollection<TeamViewModel>> GetUserTeamsAsync<T>(T teamsIds) where T : ICollection<TeamViewModel>
    {
        var filter = new BaseFilter(teamsIds.Select(c => c.Id).ToArray());
        var teamViewModels = await apiService.SendGetMessages<TeamDto, TeamViewModel>(filter: filter);
        return teamViewModels;
    }

    public async Task<ICollection<ProjectTaskViewModel>> GetUserTasksAsync<T>(T tasksIds)where T : ICollection<BaseViewModel>
    {
        var filter = new BaseFilter(tasksIds.Select(c => c.Id).ToArray());
        var taskViewModels = await apiService.SendGetMessages<ProjectTaskDto, ProjectTaskViewModel>(filter: filter);
        return taskViewModels;
    }

    public async Task<ICollection<ProjectTaskViewModel>> GetUserTasksAsync(ICollection<int> tasksIds)
    {
        var filter = new BaseFilter(tasksIds.Distinct().ToArray());
        var taskViewModels = await apiService.SendGetMessages<ProjectTaskDto, ProjectTaskViewModel>(filter: filter);
        return taskViewModels;
    }
}