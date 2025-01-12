using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class ProjectTaskPageService(
    IGateWayApiService apiService, 
    IMapper mapper,
    IBaseModelService<TaskInfoViewModel> taskInfoService
    ) : BaseModelService<ProjectTaskDto, ProjectTaskViewModel>(apiService, mapper), IProjectTaskPageService
{
    private readonly IGateWayApiService _apiService = apiService;

    public async Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId)
    {
        var companyUserViewModel = await _apiService.SendGetMessage<CompanyUserDto, CompanyUserViewModel>(userCompanyId);
        return companyUserViewModel;
    }

    public async Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<int> ids)
    {
        var filter = new BaseFilter(ids);
        var userViewModels = await _apiService.SendGetMessages<UserDto, UserViewModel>(filter: filter);
        return userViewModels;
    }
    
    public async Task<ICollection<TaskInfoViewModel>> GetTaskInfoAsync(ICollection<int> ids)
    {
        var filter = new BaseFilter(ids);
        var taskInfoViewModels = await _apiService.SendGetMessages<TaskInfoDto, TaskInfoViewModel>(filter: filter);
        return taskInfoViewModels;
    }
    
    public async Task AddTaskInfoAsync(TaskInfoViewModel taskInfo)
    {
        await taskInfoService.CreateValueAsync(taskInfo);
    }
}