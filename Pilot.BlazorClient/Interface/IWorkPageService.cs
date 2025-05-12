using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IWorkPageService
{
    Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId);

    Task<CompanyViewModel> GetCompanyAsync(int companyId);

    Task FillProjectsAsync(ICollection<ProjectViewModel> userProjects);

    Task<ICollection<ProjectViewModel>> GetProjectsAsync(int companyId);

    Task FillTeamsAsync(ICollection<ProjectViewModel> projects);

    Task FillCompanyUsersAsync(ICollection<ProjectViewModel> projects);

    Task<ICollection<ProjectTaskViewModel>> GetUserTasksAsync(int companyUserId);

    Task<ICollection<CompanyUserViewModel>> GetCompanyEmployeesAsync(int companyId);

    Task<ICollection<JobApplicationViewModel>> GetCompanyJobApplicationsAsync(int companyId);

    Task<ICollection<CompanyUserViewModel>> GetJobApplicationUsersAsync(
        ICollection<JobApplicationViewModel> jobApplications);

    Task<ICollection<PostViewModel>> GetPostsAsync(int companyId);

    Task<ICollection<TeamViewModel>> GetTeamsAsync(int companyId);

    Task AddTeamAsync(TeamViewModel team);

    Task AddProjectAsync(ProjectViewModel project);

    Task<ICollection<ProjectTaskViewModel>> GetUserProjectTasksAsync(int userId);
    
    Task<ICollection<TaskInfoViewModel>> GetDaylyAcitvityAsync(int companyUserId);

    Task<ICollection<ProjectTaskViewModel>> FillProjectsIntoTeamsAsync(ICollection<ProjectTaskViewModel> projectTasks);

    Task<ICollection<ProjectTaskViewModel>> GetCompanyTasksAsync(int companyId);

    Task<ICollection<ProjectViewModel>> GetTaskManagementProjectsAsync(int companyId);
    Task<ICollection<ProjectTaskViewModel>> GetTaskManagementCompanyTasksAsync(int companyId);

    Task UpdateProjectAsync(ProjectViewModel project);

    Task UpdateTeamAsync(TeamViewModel team);

    Task UpdateCompanyAsync(CompanyViewModel company);

    Task DeleteProjectAsync(int projectId);

    Task DeleteTeamAsync(int teamId);

    Task AssignEmployeeToTeamAsync(int teamId, int employeeId);

    Task RemoveTeamEmployeeAsync(int teamEmployeeId, int employeeId);

    Task UpdateEmployeeAsync(CompanyUserViewModel companyUser);

    Task UploadFileAsync(FileViewModel file);
    
    Task UpdateTaskStatusAsync(ProjectTaskViewModel task);
}