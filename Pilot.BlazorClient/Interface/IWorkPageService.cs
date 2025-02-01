using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IWorkPageService
{
    Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId);

    Task<CompanyViewModel> GetCompanyAsync(int companyId);

    Task FillProjectsAsync(ICollection<ProjectViewModel> userProjects);

    Task<ICollection<ProjectViewModel>> GetProjectsAsync(int companyId);

    Task<ICollection<TeamViewModel>> GetUserTeamsAsync<T>(T userProjects) where T : ICollection<TeamViewModel>;

    Task FillTeamsAsync(ICollection<ProjectViewModel> projects);

    Task FillCompanyUsersAsync(ICollection<ProjectViewModel> projects);

    Task<ICollection<ProjectTaskViewModel>> GetUserTasksAsync(ICollection<int> tasksIds,
        int? projectId = null);

    Task<ICollection<CompanyUserViewModel>> GetCompanyEmployeesAsync(int companyId);

    Task<ICollection<JobApplicationViewModel>> GetCompanyJobApplicationsAsync(int companyId);
    
    Task<ICollection<CompanyUserViewModel>> GetJobApplicationUsersAsync(
        ICollection<JobApplicationViewModel> jobApplications);
}