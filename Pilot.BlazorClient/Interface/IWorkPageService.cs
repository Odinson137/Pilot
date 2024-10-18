using System.Collections;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Interface;

public interface IWorkPageService
{
    Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId);
    
    Task<ICollection<ProjectViewModel>> GetUserProjectsAsync<T>(T userProjects) where T : ICollection<BaseViewModel>;
    
    Task<ICollection<ProjectViewModel>> GetUserProjectsAsync(ICollection<int> userProjects);
    
    Task<ICollection<TeamViewModel>> GetUserTeamsAsync(ICollection<int> userProjects);
    
    Task<ICollection<TeamViewModel>> GetUserTeamsAsync<T>(T userProjects) where T : ICollection<TeamViewModel>;

    Task<ICollection<ProjectTaskViewModel>> GetUserTasksAsync<T>(T tasksIds) where T : ICollection<BaseViewModel>;
    
    Task<ICollection<ProjectTaskViewModel>> GetUserTasksAsync(ICollection<int> tasksIds);
}
