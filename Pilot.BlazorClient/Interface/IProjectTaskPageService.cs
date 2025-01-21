using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IProjectTaskPageService : IBasePageService<ProjectTaskViewModel>
{
    Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId);
    
    Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<int> ids);

    Task<ICollection<TaskInfoViewModel>> GetTaskInfoAsync(ICollection<int> ids);

    Task AddTaskInfoAsync(TaskInfoViewModel taskInfo);
}
