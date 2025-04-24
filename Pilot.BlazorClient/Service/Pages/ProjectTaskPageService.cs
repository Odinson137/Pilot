using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Service.Pages;

public class ProjectTaskPageService(
    IBaseModelService<ProjectTaskViewModel> projectTaskService, 
    IBaseModelService<UserViewModel> userService, 
    IBaseModelService<CompanyUserViewModel> companyUserService, 
    IBaseModelService<TaskInfoViewModel> taskInfoService,
    IBaseModelService<TeamViewModel> teamService,
    IMessengerService messengerService,
    IBaseModelService<FileViewModel> fileService
    ) : BasePageService<ProjectTaskViewModel>(projectTaskService, messengerService), IProjectTaskPageService
{

    public async Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId)
    {
        var companyUserViewModel = await companyUserService.GetValueAsync(userCompanyId);
        return companyUserViewModel;
    }

    public Task<CompanyUserViewModel> GetCompanyUserAsync(int userId)
    {
        return companyUserService.GetValueAsync(userId);
    }

    public async Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<int> ids)
    {
        var userViewModels = await userService.GetValuesAsync(ids);
        return userViewModels;
    }
    
    public async Task<ICollection<TaskInfoViewModel>> GetTaskInfoAsync(ICollection<int> ids)
    {
        var taskInfoViewModels = await taskInfoService.GetValuesAsync(ids);
        return taskInfoViewModels;
    }
    
    public async Task AddTaskInfoAsync(TaskInfoViewModel taskInfo)
    {
        await taskInfoService.CreateValueAsync(taskInfo);
    }

    public async Task UploadFileAsync(FileViewModel file)
    {
        await fileService.CreateValueAsync(file);
    }

    public async Task AddProjectTaskAsync(ProjectTaskViewModel task)
    {
        await projectTaskService.CreateValueAsync(task);
    }

    public async Task<ICollection<TeamViewModel>> GetProjectTeamsAsync(int projectId)
    {
        return await teamService.GetValuesAsync(c => c.Project.Id, projectId);
    }
}