using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.Service.Pages;

public class NewUserApplicationPageService(
    IUserService userService,
    IBaseModelService<JobApplicationViewModel> jobApplicationService,
    IBaseModelService<CompanyPostViewModel> companyPostService,
    IBaseModelService<PostViewModel> postService,
    IBaseModelService<CompanyViewModel> companyService
) : INewUserApplicationPageService
{
    public async Task<ICollection<JobApplicationViewModel>> GetUserJobApplicationsAsync()
    {
        var user = await userService.GetCurrentUserAsync();
        var applications = await jobApplicationService.GetValuesAsync(c => c.UserId, user.Id);
        var companyPosts = await companyPostService.GetValuesAsync(applications.Select(c => c.CompanyPost.Id).ToList());
        var posts = await postService.GetValuesAsync(companyPosts.Select(c => c.Post.Id).ToList());
        var companies = await companyService.GetValuesAsync(posts.Select(c => c.CompanyId).ToList());
        foreach (var jobApplicationViewModel in applications)
        {
            jobApplicationViewModel.CompanyPost = companyPosts.First(c => c.Id == jobApplicationViewModel.CompanyPost.Id);
            jobApplicationViewModel.CompanyPost.Post = posts.First(c => c.Id == jobApplicationViewModel.CompanyPost.Post.Id);
            jobApplicationViewModel.CompanyPost.Post.Company = companies.First(c => c.Id == jobApplicationViewModel.CompanyPost.Post.CompanyId);
        }
        return applications;
    }

    public async Task DeleteApplicationAsync(int applicationId, Action<InfoMessageViewModel>? action = null)
    {
        var application = await jobApplicationService.GetValueAsync(applicationId);
        application.Status = ApplicationStatus.Canceled;
        await jobApplicationService.UpdateValueAsync(application, action);
    }
}