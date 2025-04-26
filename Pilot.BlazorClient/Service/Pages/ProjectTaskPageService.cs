using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Service.Pages;

public class ProjectTaskPageService(
    IBaseModelService<ProjectTaskViewModel> projectTaskService, 
    IBaseModelService<TeamEmployeeViewModel> teamEmployeeService, 
    IBaseModelService<UserViewModel> userService, 
    IBaseModelService<CompanyUserViewModel> companyUserService, 
    IBaseModelService<TaskInfoViewModel> taskInfoService,
    IBaseModelService<TeamViewModel> teamService,
    IMessengerService messengerService,
    IBaseModelService<FileViewModel> fileService
    ) : BasePageService<ProjectTaskViewModel>(projectTaskService, messengerService), IProjectTaskPageService
{
    public async Task<ProjectTaskViewModel> GetTaskAsync(int taskId)
    {
        var task = await projectTaskService.GetValueAsync(taskId);
        var teamEmployee = await teamEmployeeService.GetValueAsync((c => c.Id, task.TeamEmployee!.Id));
        task.TeamEmployee = teamEmployee;
        return task;
    }

    public async Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId)
    {
        var companyUserViewModel = await companyUserService.GetValueAsync(userCompanyId);
        return companyUserViewModel;
    }

    public async Task<CompanyUserViewModel> GetCompanyUserAsync(int userId)
    {
        var users = await companyUserService.GetValuesAsync(c => c.UserId, userId);
        return users.First();
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
    
    public async Task AddTaskInfoAsync(TaskInfoViewModel taskInfo, Action<InfoMessageViewModel>? action = null)
    {
        await taskInfoService.CreateValueAsync(taskInfo, action);
    }

    public async Task UploadFileAsync(FileViewModel file)
    {
        await fileService.CreateValueAsync(file);
    }

    public async Task AddProjectTaskAsync(ProjectTaskViewModel task, int companyUserId, int selectedTeamId, Action<InfoMessageViewModel>? action = null)
    {
        var teamEmployee = await teamEmployeeService.GetValueAsync((c => c.Team.Id, selectedTeamId), (c => c.CompanyUser.Id, companyUserId));
        task.TeamEmployee = teamEmployee;
        await CreateValueAsync(task, action);
    }

    public async Task<ICollection<TeamViewModel>> GetProjectTeamsAsync(int projectId)
    {
        return await teamService.GetValuesAsync(c => c.Project!.Id, projectId);
    }
}