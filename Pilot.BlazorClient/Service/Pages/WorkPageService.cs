using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Service.Pages;

public class WorkPageService(
    IBaseModelService<CompanyUserViewModel> companyUserService,
    IBaseModelService<CompanyViewModel> companyService,
    IBaseModelService<ProjectTaskViewModel> projectTaskService,
    IBaseModelService<UserViewModel> userBaseService,
    IBaseModelService<TeamViewModel> teamService,
    IBaseModelService<JobApplicationViewModel> jobApplicationService,
    IBaseModelService<CompanyPostViewModel> companyPostService,
    IBaseModelService<PostViewModel> postService,
    IBaseModelService<ProjectViewModel> projectService
) : IWorkPageService
{
    public async Task<CompanyUserViewModel> GetUserCompanyAsync(int userCompanyId)
    {
        var companyUserViewModel = await companyUserService.GetValueAsync(userCompanyId);
        return companyUserViewModel;
    }

    public async Task<CompanyViewModel> GetCompanyAsync(int companyId)
    {
        return await companyService.GetValueAsync(companyId);
    }

    public async Task FillProjectsAsync(ICollection<ProjectViewModel> userProjects)
    {
        var filter = new BaseFilter(userProjects.Select(c => c.Id).ToArray());
        userProjects.Clear();
        var values = await projectService.GetValuesAsync(filter: filter);
        foreach (var model in values)
            userProjects.Add(model);
    }

    public async Task<ICollection<ProjectViewModel>> GetProjectsAsync(int companyId)
    {
        var projectViewModels =
            await projectService.GetValuesAsync(predicate: c => c.Company.Id, value: companyId);
        var teams =
            await teamService.GetValuesAsync(projectViewModels.SelectMany(c => c.Teams.Select(x => x.Id)).ToList());
        foreach (var projectViewModel in projectViewModels)
        {
            projectViewModel.Teams = teams.Where(c =>
                projectViewModel.Teams.Select(x => x.Id).Contains(c.Id)).ToList();
        }

        return projectViewModels;
    }

    public async Task FillTeamsAsync(ICollection<ProjectViewModel> projects)
    {
        var teamViewModels =
            await teamService.GetValuesAsync(
                projects.SelectMany(c => c.Teams).Select(c => c.Id).Distinct().ToArray());
        foreach (var project in projects)
        {
            var ids = project.Teams.Select(c => c.Id).ToList();
            project.Teams = teamViewModels.Where(c => ids.Contains(c.Id)).ToList();
        }
    }

    public async Task FillCompanyUsersAsync(ICollection<ProjectViewModel> projects)
    {
        var filter = new BaseFilter(projects.SelectMany(c => c.Teams).SelectMany(c => c.CompanyUsers).Select(c => c.Id)
            .Distinct().ToArray());
        var task1 = companyUserService.GetValuesAsync(filter);
        var task2 = userBaseService.GetValuesAsync(filter);

        await Task.WhenAll(task1, task2);

        var companyUsers = task1.Result;
        var users = task2.Result;
        foreach (var companyUser in companyUsers)
        {
            companyUser.User = users.First(c => c.Id == companyUser.Id);
        }

        foreach (var project in projects)
        {
            foreach (var team in project.Teams)
            {
                var ids = team.CompanyUsers.Select(c => c.Id).ToList();
                team.CompanyUsers = companyUsers.Where(c => ids.Contains(c.Id)).ToList();
            }
        }
    }

    public async Task<ICollection<TeamViewModel>> GetUserTeamsAsync<T>(T teamsIds) where T : ICollection<TeamViewModel>
    {
        var filter = new BaseFilter(teamsIds.Select(c => c.Id).ToArray());
        var teamViewModels = await teamService.GetValuesAsync(filter: filter);
        return teamViewModels;
    }

    public async Task<ICollection<ProjectTaskViewModel>> GetUserTasksAsync(ICollection<int> tasksIds,
        int? projectId = null)
    {
        var filter = new BaseFilter
        {
            Ids = tasksIds.Distinct().ToArray(),
            Skip = 0,
            Take = int.MaxValue,
            WhereFilter = new WhereFilter()
        };

        if (projectId != null)
        {
            filter.WhereFilter = new WhereFilter();
            filter.WhereFilter.Init<int, ProjectTaskViewModel>((c => c.Team.Project.Id, projectId.Value));
        }

        var taskViewModels = await projectTaskService.GetValuesAsync(filter);
        var teamViewModels =
            await teamService.GetValuesAsync(taskViewModels.Select(c => c.Team.Id).Distinct().ToArray());
        foreach (var taskViewModel in taskViewModels)
            taskViewModel.Team = teamViewModels.First(c => c.Id == taskViewModel.Team.Id);
        return taskViewModels;
    }

    public async Task<ICollection<CompanyUserViewModel>> GetCompanyEmployeesAsync(int companyId)
    {
        var companyUsers =
            await companyUserService.GetValuesAsync(predicate: c => c.Company.Id, companyId);
        var users =
            await userBaseService.GetValuesAsync(companyUsers.Select(c => c.Id).ToArray());
        foreach (var companyUser in companyUsers)
            companyUser.User = users.First(c => c.Id == companyUser.Id);


        return companyUsers;
    }

    public async Task<ICollection<JobApplicationViewModel>> GetCompanyJobApplicationsAsync(int companyId)
    {
        var jobApplications = await jobApplicationService.GetValuesAsync(c => c.CompanyPost.Post.CompanyId, companyId);
        var companyPosts =
            await companyPostService.GetValuesAsync(jobApplications.Select(c => c.CompanyPost.Id).ToList());
        var posts = await postService.GetValuesAsync(companyPosts.Select(c => c.Post.Id).ToList());

        foreach (var companyPost in companyPosts)
            companyPost.Post = posts.First(c => c.Id == companyPost.Post.Id);

        foreach (var jobApplication in jobApplications)
            jobApplication.CompanyPost = companyPosts.First(c => c.Id == jobApplication.CompanyPost.Id);

        return jobApplications;
    }

    public async Task<ICollection<CompanyUserViewModel>> GetJobApplicationUsersAsync(
        ICollection<JobApplicationViewModel> jobApplications)
    {
        var companyUsers = await companyUserService.GetValuesAsync(jobApplications.Select(c => c.UserId).ToList());
        var users = await userBaseService.GetValuesAsync(companyUsers.Select(c => c.Id).ToList());
        foreach (var companyUser in companyUsers)
            companyUser.User = users.First(c => c.Id == companyUser.Id);
        return companyUsers;
    }
    
    public async Task<ICollection<PostViewModel>> GetPostsAsync(int companyId)
    {
        var posts = await postService.GetValuesAsync(predicate: c => c.CompanyId, companyId);
        return posts;
    }
}