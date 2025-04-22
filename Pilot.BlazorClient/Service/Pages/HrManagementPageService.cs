using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.HelperViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.FullDto;

namespace Pilot.BlazorClient.Service.Pages;

public class HrManagementPageService(
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
) : IHrManagementPageService
{
    public Task<List<PostViewModel>> GetPositionsAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public Task AddPositionAsync(PostViewModel position)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePositionAsync(PostViewModel position)
    {
        throw new NotImplementedException();
    }

    public Task DeletePositionAsync(int positionId)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<CompanyPostViewModel>> GetPostsAsync(int companyId)
    {
        var companyPosts = await companyPostService.GetValuesAsync(c => c.Post.CompanyId, companyId);
        var posts = await postService.GetValuesAsync(companyPosts.Select(c => c.Post.Id).ToList());
        foreach (var companyPostViewModel in companyPosts)
            companyPostViewModel.Post = posts.Single(c => c.Id == companyPostViewModel.Post.Id);
        return companyPosts;
    }

    public Task AddPostAsync(CompanyPostViewModel post)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePostAsync(CompanyPostViewModel post)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostAsync(int postId)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<JobApplicationViewModel>> GetCompanyJobApplicationsAsync(int companyId)
    {
        var jobApplications = await jobApplicationService.GetValuesAsync(c => c.CompanyPost.Post.CompanyId, companyId);
        var users = await userBaseService.GetValuesAsync(jobApplications.Select(c => c.UserId).ToList());
        var companyPosts = await companyPostService.GetValuesAsync(jobApplications.Select(c => c.CompanyPost.Id).ToList());
        var posts = await postService.GetValuesAsync(companyPosts.Select(c => c.Post.Id).ToList());
        foreach (var jobApplicationViewModel in jobApplications)
        {
            jobApplicationViewModel.User = users.Single(c => c.Id == jobApplicationViewModel.UserId);
            jobApplicationViewModel.CompanyPost = companyPosts.Single(c => c.Id == jobApplicationViewModel.CompanyPost.Id);
            jobApplicationViewModel.CompanyPost.Post = posts.Single(c => c.Id == jobApplicationViewModel.CompanyPost.Post.Id);
        }
        return jobApplications;
    }

    public Task UpdateApplicationStatusAsync(JobApplicationViewModel application)
    {
        throw new NotImplementedException();
    }

    public Task<List<SkillViewModel>> GetAvailableSkillsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<ProjectViewModel>> GetProjectsAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public Task<CompanyUserViewModel> GetUserCompanyAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CompanyUserViewModel>> GetCompanyEmployeesAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProjectTaskViewModel>> GetUserTasksAsync(List<int> projectIds, int? userId)
    {
        throw new NotImplementedException();
    }
}