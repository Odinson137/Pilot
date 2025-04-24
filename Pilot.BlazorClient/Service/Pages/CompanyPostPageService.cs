using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Service.Pages;

public class CompanyPostPageService(
    IBaseModelService<CompanyPostViewModel> companyPostService,
    IBaseModelService<CompanyViewModel> companyService,
    IBaseModelService<SkillViewModel> skillService,
    IBaseModelService<JobApplicationViewModel> jobApplicationViewModelService,
    IMessengerService messengerService,
    IBaseModelService<PostViewModel> postService
) : BasePageService<CompanyPostViewModel>(companyPostService, messengerService), ICompanyPostPageService
{
    private readonly IBaseModelService<CompanyPostViewModel> _companyPostService = companyPostService;

    public async Task<ICollection<CompanyPostViewModel>> GetVacanciesAsync(int skip, int take, string? search = null)
    {
        var companyPosts = await _companyPostService.GetValuesAsync(skip, take);
        var posts = await postService.GetValuesAsync(companyPosts.Select(c => c.Id).ToArray());
        foreach (var companyPost in companyPosts)
        {
            companyPost.Post = posts.First(p => p.Id == companyPost.Post.Id);
        }

        return companyPosts;
    }

    public async Task<CompanyPostViewModel> GetVacancyAsync(int vacancyId)
    {
        var companyPost = await GetValueAsync(vacancyId);
        companyPost.Post = await postService.GetValueAsync(companyPost.Post.Id);
        return companyPost;
    }

    public async Task<ICollection<SkillViewModel>> GetPostSkillsAsync(PostViewModel post)
    {
        var skills = await skillService.GetValuesAsync(post.Skills.Select(s => s.Id).ToArray());
        return skills;
    }


    public async Task<CompanyViewModel> GetCompanyAsync(int companyId)
    {
        var companyViewModel = await companyService.GetValueAsync(companyId);
        return companyViewModel;
    }

    public async Task SubmitApplicationAsync(int vacancyId, string letter)
    {
        var jobApplication = new JobApplicationViewModel
        {
            CompanyPost = new CompanyPostViewModel { Id = vacancyId },
            Message = letter
        };

        await jobApplicationViewModelService.CreateValueAsync(jobApplication);
    }
}