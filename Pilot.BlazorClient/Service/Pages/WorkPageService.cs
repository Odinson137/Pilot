using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

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
    IBaseModelService<ProjectViewModel> projectService,
    IBaseModelService<TeamEmployeeViewModel> teamEmployeeViewModel,
    IBaseModelService<FileViewModel> fileService,
    IBaseModelService<TeamEmployeeViewModel> teamEmployeeService
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
            companyUser.User = users.First(c => c.Id == companyUser.UserId);
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

    public async Task<ICollection<ProjectTaskViewModel>> GetUserTasksAsync(int companyUserId)
    {
        var taskViewModels =
            await projectTaskService.GetValuesAsync(c => c.TeamEmployee!.CompanyUser.Id, companyUserId);
        var teamEmployees =
            await teamEmployeeService.GetValuesAsync(taskViewModels.Select(c => c.TeamEmployee!.Id).ToList());
        foreach (var taskViewModel in taskViewModels)
            taskViewModel.TeamEmployee = teamEmployees.First(c => c.Id == taskViewModel.TeamEmployee!.Id);

        var teamViewModels =
            await teamService.GetValuesAsync(taskViewModels.Select(c => c.TeamEmployee!.Team.Id).Distinct().ToArray());
        foreach (var taskViewModel in taskViewModels)
            taskViewModel.TeamEmployee!.Team = teamViewModels.First(c => c.Id == taskViewModel.TeamEmployee!.Team.Id);
        return taskViewModels;
    }

    public async Task<ICollection<CompanyUserViewModel>> GetCompanyEmployeesAsync(int companyId)
    {
        var companyUsers =
            await companyUserService.GetValuesAsync(predicate: c => c.Company.Id, companyId);
        var users =
            await userBaseService.GetValuesAsync(companyUsers.Select(c => c.Id).ToArray());
        var company = await companyService.GetValueAsync(companyId);

        foreach (var companyUser in companyUsers)
        {
            companyUser.User = users.First(c => c.Id == companyUser.UserId);
            companyUser.Company = company;
        }

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
            companyUser.User = users.First(c => c.Id == companyUser.UserId);
        return companyUsers;
    }

    public async Task<ICollection<PostViewModel>> GetPostsAsync(int companyId)
    {
        var posts = await postService.GetValuesAsync(predicate: c => c.CompanyId, companyId);
        return posts;
    }

    public async Task<ICollection<TeamViewModel>> GetTeamsAsync(int companyId)
    {
        var teams = await teamService.GetValuesAsync(predicate: c => c.Project.Company.Id, companyId);
        return teams;
    }

    public async Task AddTeamEmployeeAsync(TeamEmployeeViewModel teamEmployee)
    {
        await teamEmployeeService.CreateValueAsync(teamEmployee);
    }

    public async Task AddTeamAsync(TeamViewModel team)
    {
        await teamService.CreateValueAsync(team);
    }

    public async Task UpdateTeamAsync(TeamViewModel team)
    {
        await teamService.UpdateValueAsync(team);
    }

    public async Task UpdateCompanyAsync(CompanyViewModel company)
    {
        await companyService.UpdateValueAsync(company);
    }

    public async Task DeleteProjectAsync(int projectId)
    {
        await projectService.DeleteValueAsync(projectId);
    }

    public async Task DeleteTeamAsync(int teamId)
    {
        await teamService.DeleteValueAsync(teamId);
    }

    public async Task RemoveTeamEmployeeAsync(int teamEmployeeId, int employeeId)
    {
        var filter = new BaseFilter
        {
            WhereFilter = new WhereFilter()
        };
        filter.WhereFilter.Init<int, TeamEmployeeViewModel>((c => c.Team.Id, teamEmployeeId));
        filter.WhereFilter.Init<int, TeamEmployeeViewModel>((c => c.CompanyUser.Id, employeeId));
        var teamEmployee = (await teamEmployeeService.GetValuesAsync(filter)).First();
        await teamEmployeeService.DeleteValueAsync(teamEmployee.Id);
    }

    public async Task AddProjectAsync(ProjectViewModel project)
    {
        await projectService.CreateValueAsync(project);
    }

    public async Task UpdateProjectAsync(ProjectViewModel project)
    {
        await projectService.UpdateValueAsync(project);
    }

    public async Task UpdateEmployeeAsync(CompanyUserViewModel companyUser)
    {
        await companyUserService.UpdateValueAsync(companyUser);
    }


    public async Task AssignEmployeeToTeamAsync(int teamId, int employeeId)
    {
        await teamEmployeeViewModel.CreateValueAsync(new TeamEmployeeViewModel
        {
            Team = new TeamViewModel { Id = teamId },
            CompanyUser = new CompanyUserViewModel { Id = employeeId }
        });
    }

    public async Task<ICollection<ProjectTaskViewModel>> GetUserProjectTasksAsync(int userId)
    {
        var result =
            await projectTaskService.GetValuesAsync(c => c.TeamEmployee!.CompanyUser.Id, userId);
        return result;
    }

    public async Task<ICollection<ProjectTaskViewModel>> GetCompanyTasksAsync(int companyId)
    {
        var result =
            await projectTaskService.GetValuesAsync(c => c.TeamEmployee!.CompanyUser.Company.Id, companyId);
        return result;
    }

    public async Task<ICollection<ProjectTaskViewModel>> FillProjectsIntoTeamsAsync(
        ICollection<ProjectTaskViewModel> projectTasks)
    {
        var ids = projectTasks.Select(c => c.TeamEmployee!.Team.Id).ToList();
        var result = await teamService.GetValuesAsync(ids);
        foreach (var projectTask in projectTasks)
            projectTask.TeamEmployee!.Team = result.First(c => c.Id == projectTask.TeamEmployee.Team.Id);
        return projectTasks;
    }

    public async Task<ICollection<ProjectViewModel>> GetTaskManagementProjectsAsync(int companyId)
    {
        var projectViewModels = await projectService.GetValuesAsync(
            c => c.Company.Id,
            companyId);
        var teams = await teamService.GetValuesAsync(
            projectViewModels.SelectMany(c => c.Teams.Select(x => x.Id)).ToList());
        var employees = await companyUserService.GetValuesAsync(
            teams.SelectMany(c => c.CompanyUsers.Select(x => x.Id)).ToList());
        var users = await userBaseService.GetValuesAsync(
            employees.Select(c => c.UserId).ToList());
        foreach (var companyUserViewModel in employees)
            companyUserViewModel.User = users.First(c => c.Id == companyUserViewModel.UserId);

        foreach (var projectViewModel in projectViewModels)
        {
            projectViewModel.Teams = teams.Where(c =>
                projectViewModel.Teams.Select(x => x.Id).Contains(c.Id)).ToList();
            foreach (var teamViewModel in projectViewModel.Teams)
            {
                teamViewModel.CompanyUsers =
                    employees.Where(c => teamViewModel.CompanyUsers.Select(x => x.Id).Contains(c.Id)).ToList();
            }
        }

        return projectViewModels;
    }

    public async Task<ICollection<ProjectTaskViewModel>> GetTaskManagementCompanyTasksAsync(int companyId)
    {
        // Получаем все задачи компании
        var tasks = await projectTaskService.GetValuesAsync(
            predicate: t => t.TeamEmployee!.CompanyUser.Company.Id,
            companyId);

        var teamEmployee = await teamEmployeeService.GetValuesAsync(tasks.Select(c => c.TeamEmployee!.Id).ToList());
        foreach (var task in tasks)
            task.TeamEmployee = teamEmployee.Single(c => c.Id == task.TeamEmployee!.Id);

        var teamIds = tasks.Select(t => t.TeamEmployee!.Team.Id).Select(id => id).Distinct().ToList();
        var userIds = tasks.Select(t => t.TeamEmployee!.CompanyUser.Id).Select(id => id).Distinct()
            .ToList();

        // Параллельно запрашиваем команды, пользователей и их данные
        var teamsTask = teamIds.Any()
            ? teamService.GetValuesAsync(teamIds)
            : Task.FromResult<ICollection<TeamViewModel>>(new List<TeamViewModel>());
        var companyUsersTask = userIds.Any()
            ? companyUserService.GetValuesAsync(userIds)
            : Task.FromResult<ICollection<CompanyUserViewModel>>(new List<CompanyUserViewModel>());
        var usersTask = userIds.Any()
            ? userBaseService.GetValuesAsync(userIds)
            : Task.FromResult<ICollection<UserViewModel>>(new List<UserViewModel>());

        await Task.WhenAll(teamsTask, companyUsersTask, usersTask);

        var teams = teamsTask.Result;
        var companyUsers = companyUsersTask.Result;
        var users = usersTask.Result;

        // Получаем проекты для команд
        var projectIds = teams.Select(t => t.Project!.Id).Select(id => id).Distinct().ToList();
        var projects = projectIds.Any()
            ? await projectService.GetValuesAsync(projectIds)
            : new List<ProjectViewModel>();

        // Заполняем данные пользователей в CompanyUser
        foreach (var companyUser in companyUsers)
            companyUser.User = users.FirstOrDefault(u => u.Id == companyUser.UserId) ?? companyUser.User;

        // Заполняем данные в задачах
        foreach (var task in tasks)
        {
            task.TeamEmployee!.CompanyUser =
                companyUsers.FirstOrDefault(u => u.Id == task.TeamEmployee.CompanyUser.Id) ??
                task.TeamEmployee.CompanyUser;
            task.TeamEmployee!.Team = teams.First(u => u.Id == task.TeamEmployee.Team.Id);
            task.TeamEmployee!.Team.Project = projects.First(u => u.Id == task.TeamEmployee.Team.Project!.Id);
        }

        return tasks;
    }

    public async Task UploadFileAsync(FileViewModel file)
    {
        await fileService.CreateValueAsync(file);
    }

    public async Task UpdateTaskStatusAsync(ProjectTaskViewModel task)
    {
        await projectTaskService.UpdateValueAsync(task);
    }
}