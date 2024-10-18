using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class ProjectTaskPageService(IGateWayApiService apiService) : IProjectTaskPageService
{
    public async Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId)
    {
        var companyUserViewModel = await apiService.SendGetMessage<CompanyUserDto, CompanyUserViewModel>(userCompanyId);
        return companyUserViewModel;
    }

    public async Task<ProjectTaskViewModel> GetUserTaskAsync(int tasksId)
    {
        var taskViewModel = await apiService.SendGetMessage<ProjectTaskDto, ProjectTaskViewModel>(tasksId);
        return taskViewModel;
    }

    public async Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<int> ids)
    {
        var filter = new BaseFilter(ids);
        var userViewModels = await apiService.SendGetMessages<UserDto, UserViewModel>(filter: filter);
        return userViewModels;
    }
    
    public async Task<ICollection<TaskInfoViewModel>> GetTaskInfoAsync(ICollection<int> ids)
    {
        var filter = new BaseFilter(ids);
        var taskInfoViewModels = await apiService.SendGetMessages<TaskInfoDto, TaskInfoViewModel>(filter: filter);
        return taskInfoViewModels;
    }
}