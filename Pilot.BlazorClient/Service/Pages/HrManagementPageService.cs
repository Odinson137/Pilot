using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Service.Pages;

public class HrManagementPageService(
    IBaseModelService<UserViewModel> userBaseService,
    IBaseModelService<JobApplicationViewModel> jobApplicationService,
    IBaseModelService<CompanyPostViewModel> companyPostService,
    IBaseModelService<PostViewModel> postService,
    IBaseModelService<SkillViewModel> skillService
) : IHrManagementPageService
{
    public async Task<ICollection<PostViewModel>> GetPositionsAsync(int companyId)
    {
        var positions = await postService.GetValuesAsync(c => c.CompanyId, companyId);
        var skills = await skillService.GetValuesAsync(positions.SelectMany(c => c.Skills).Select(x => x.Id).ToList());
        foreach (var postViewModel in positions)
        {
            postViewModel.Skills = skills.Where(c => postViewModel.Skills.Select(x => x.Id).Contains(c.Id)).ToList();
        }
        return positions;
    }

    public async Task AddPositionAsync(PostViewModel position)
    {
        await postService.CreateValueAsync(position);
    }

    public async Task UpdatePositionAsync(PostViewModel position)
    {
        await postService.UpdateValueAsync(position);

    }

    public async Task DeletePositionAsync(int positionId)
    {
        await companyPostService.DeleteValueAsync(positionId);
    }

    public async Task<ICollection<CompanyPostViewModel>> GetPostsAsync(int companyId)
    {
        var companyPosts = await companyPostService.GetValuesAsync(c => c.Post.CompanyId, companyId);
        var posts = await postService.GetValuesAsync(companyPosts.Select(c => c.Post.Id).ToList());
        foreach (var companyPostViewModel in companyPosts)
            companyPostViewModel.Post = posts.Single(c => c.Id == companyPostViewModel.Post.Id);
        return companyPosts;
    }

    public async Task AddPostAsync(CompanyPostViewModel post)
    {
        await companyPostService.CreateValueAsync(post);
    }

    public async Task UpdatePostAsync(CompanyPostViewModel post)
    {
        await companyPostService.UpdateValueAsync(post);

    }

    public async Task DeletePostAsync(int postId)
    {
        await postService.DeleteValueAsync(postId);
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

    public async Task UpdateApplicationStatusAsync(JobApplicationViewModel application)
    {
        await jobApplicationService.UpdateValueAsync(application);
    }

    public async Task<ICollection<SkillViewModel>> GetAvailableSkillsAsync()
    {
        return await skillService.GetValuesAsync();
    }
}