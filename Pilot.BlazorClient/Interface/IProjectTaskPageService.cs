using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IProjectTaskPageService : IBasePageService<ProjectTaskViewModel>
{
    Task<ProjectTaskViewModel> GetTaskAsync(int taskId);
    
    Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId);
    
    Task<CompanyUserViewModel> GetCompanyUserAsync(int userId);
    
    Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<int> ids);

    Task<ICollection<TaskInfoViewModel>> GetTaskInfoAsync(ICollection<int> ids);

    Task AddTaskInfoAsync(TaskInfoViewModel taskInfo, Action<InfoMessageViewModel>? action = null);

    Task UploadFileAsync(FileViewModel fileV);
    
    Task AddProjectTaskAsync(ProjectTaskViewModel task, int userId, int selectedTeamId,
        Action<InfoMessageViewModel>? action = null);
    
    Task<ICollection<TeamViewModel>> GetProjectTeamsAsync(int projectId);
    
    Task<List<AuditHistoryViewModel>> GetHistoryAsync(int projectTaskId);
}
