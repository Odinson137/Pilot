using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Service.Pages;

public class CompanyPostPageService(
    IBaseModelService<CompanyPostViewModel> companyPostService,
    IBaseModelService<CompanyViewModel> companyService,
    IBaseModelService<CompanyUserViewModel> companyUserService,
    IUserService userService,
    IBaseModelService<SkillViewModel> skillService,
    IBaseModelService<JobApplicationViewModel> jobApplicationViewModelService,
    IMessengerService messengerService,
    IBaseModelService<PostViewModel> postService
) : BasePageService<CompanyPostViewModel>(companyPostService, messengerService), ICompanyPostPageService
{
    private readonly IBaseModelService<CompanyPostViewModel> _companyPostService = companyPostService;

    public async Task<ICollection<CompanyPostViewModel>> GetVacanciesAsync(int skip, int take, string? search = null)
    {
        var companyPosts = await _companyPostService.GetValuesAsync(c => c.IsOpen, true);
        var posts = await postService.GetValuesAsync(companyPosts.Select(c => c.Id).ToArray());
        var skills = await skillService.GetValuesAsync(posts.SelectMany(c => c.Skills.Select(x => x.Id)).ToArray());
        var companies = await companyService.GetValuesAsync(posts.Select(c => c.CompanyId).ToArray());
        foreach (var companyPost in companyPosts)
        {
            companyPost.Post = posts.First(p => p.Id == companyPost.Post.Id);
            companyPost.Post.Company = companies.First(p => p.Id == companyPost.Post.CompanyId);
            companyPost.Post.Skills =
                skills.Where(s => companyPost.Post.Skills.Select(x => x.Id).Contains(s.Id)).ToList();
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

    public async Task SubmitApplicationAsync(int vacancyId, string letter, Action<InfoMessageViewModel>? callback = null)
    {
        var user = await userService.GetCurrentUserAsync();
        var jobApplication = new JobApplicationViewModel
        {
            CompanyPost = new CompanyPostViewModel { Id = vacancyId },
            Message = letter
        };

        await jobApplicationViewModelService.CreateValueAsync(jobApplication, callback);
    }

    public async Task<bool> IsUserInCompanyAsync(int companyId)
    {
        var currentUser = await userService.GetCurrentUserAsync();
        var companyUser = await companyUserService.GetValuesAsync((c => c.UserId, currentUser.Id));
        return companyUser.Count != 0;
    }
}