using System.Collections;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Interface;

public interface IProjectTaskPageService
{
    Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId);
    
    Task<ProjectTaskViewModel> GetUserTaskAsync(int tasksId);
    
    Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<int> ids);

    Task<ICollection<TaskInfoViewModel>> GetTaskInfoAsync(ICollection<int> ids);
}
