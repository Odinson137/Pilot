using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
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

    public async Task<CompanyViewModel> GetCompanyAsync(int companyId)
    {
        return await apiService.SendGetMessage<CompanyDto, CompanyViewModel>(companyId);
    }

    public async Task FillProjectsAsync(ICollection<ProjectViewModel> userProjects)
    {
        var filter = new BaseFilter(userProjects.Select(c => c.Id).ToArray());
        userProjects.Clear();
        var values = await apiService.SendGetMessages<ProjectDto, ProjectViewModel>(filter: filter);
        foreach (var model in values)
        {
            userProjects.Add(model);
        }
    }

    public async Task<ICollection<ProjectViewModel>> GetUserProjectsAsync(ICollection<TeamViewModel> userProjects)
    {
        var filter = new BaseFilter(userProjects.Select(c => c.Id).Distinct().ToList());
        var projectViewModels = await apiService.SendGetMessages<ProjectDto, ProjectViewModel>(filter: filter);
        return projectViewModels;
    }

    public async Task FillTeamsAsync(ICollection<ProjectViewModel> projects)
    {
        var filter = new BaseFilter(projects.SelectMany(c => c.Teams).Select(c => c.Id).Distinct().ToArray());
        var teamViewModels = await apiService.SendGetMessages<TeamDto, TeamViewModel>(filter: filter);
        foreach (var project in projects)
        {
            var ids = project.Teams.Select(c => c.Id).ToList();
            project.Teams = teamViewModels.Where(c => ids.Contains(c.Id)).ToList();
        }
    }

    public async Task FillCompanyUsersAsync(ICollection<ProjectViewModel> projects)
    {
        var filter = new BaseFilter(projects.SelectMany(c => c.Teams).SelectMany(c => c.CompanyUsers).Select(c => c.Id).Distinct().ToArray());
        
        var task1 = apiService.SendGetMessages<CompanyUserDto, CompanyUserViewModel>(filter: filter);
        var task2 = apiService.SendGetMessages<UserDto, UserViewModel>(filter: filter);

        await Task.WhenAll(task1, task2);

        var companyUsers = task1.Result;
        var users = task2.Result;
        foreach (var companyUser in companyUsers)
        {
            companyUser.User = users.First(c => c.Id == companyUser.Id);
        }
        
        foreach (var project in projects)
        {
            foreach (var team in project.Teams)
            {
                var ids = team.CompanyUsers.Select(c => c.Id).ToList();
                team.CompanyUsers = companyUsers.Where(c => ids.Contains(c.Id)).ToList();
            }
        }
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