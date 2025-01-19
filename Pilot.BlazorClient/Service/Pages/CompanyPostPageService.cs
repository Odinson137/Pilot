using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class CompanyPostPageService(
    IGateWayApiService apiService, 
    IMapper mapper)
    : BaseModelService<CompanyPostDto, CompanyPostViewModel>(apiService, mapper), ICompanyPostPageService
{
    public async Task<ICollection<CompanyPostViewModel>> GetVacanciesAsync(int skip, int take, string? search = null)
    {
        var companyPosts = await GetValuesAsync(skip: skip, take: take);
        var ids = companyPosts.Select(c => c.Id).ToArray();
        var posts = await apiService.SendGetMessages<PostDto, PostViewModel>(filter: new BaseFilter(ids));
        foreach (var companyPost in companyPosts)
        {
            companyPost.Post = posts.First(p => p.Id == companyPost.Post.Id);
        }

        return companyPosts;
    }

    public async Task<CompanyPostViewModel> GetVacancyAsync(int vacancyId)
    {
        var companyPost = await GetValueAsync(vacancyId);
        companyPost.Post = await apiService.SendGetMessage<PostDto, PostViewModel>(companyPost.Post.Id);
        return companyPost;
    }
    
    public async Task<ICollection<SkillViewModel>> GetPostSkillsAsync(PostViewModel post)
    {
        var filter = new BaseFilter(post.Skills.Select(s => s.Id).ToArray());
        var skills = await apiService.SendGetMessages<SkillDto, SkillViewModel>(filter: filter);
        return skills;
    }
    
    
    public async Task<CompanyViewModel> GetCompanyAsync(int companyId)
    {
        var companyViewModel = await apiService.SendGetMessage<CompanyDto, CompanyViewModel>(companyId);
        return companyViewModel;
    }

    public async Task SubmitApplicationAsync(int vacancyId, string letter)
    {
        var jobApplication = new JobApplicationDto
        {
            CompanyPost = new BaseDto {Id = vacancyId},
            Message = letter
        };
        
        await apiService.SendPostMessage(null, jobApplication);
    }
}