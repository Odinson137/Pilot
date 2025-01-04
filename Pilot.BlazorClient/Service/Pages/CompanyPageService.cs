using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class CompanyPageService(IGateWayApiService apiService, IMapper mapper) : BaseModelService<CompanyDto, CompanyViewModel>(apiService, mapper), ICompanyPageService
{
    private readonly IGateWayApiService _apiService = apiService;

    public async Task<ICollection<ProjectViewModel>> GetProjectsAsync(ICollection<ProjectViewModel> projectsIds)
    {
        var idArray = projectsIds.Select(c => c.Id).ToArray();
        var projectViewModels =
            await _apiService.SendGetMessages<ProjectDto, ProjectViewModel>(
                filter: new BaseFilter(idArray));
        return projectViewModels;
    }
    
    public async Task<ICollection<CompanyPostViewModel>> GetOpenCompanyPostAsync(int companyId)
    {
        var filter = new BaseFilter(companyId);
        var companyPostViewModels = await _apiService.SendGetMessages<CompanyPostDto, CompanyPostViewModel>(Urls.OpenCompanyPost, filter);

        var postArray = companyPostViewModels.Select(c => c.Post.Id).ToArray();

        var postFilter = new BaseFilter(postArray);
        var posts = await _apiService.SendGetMessages<PostDto, PostViewModel>(null, postFilter);
        
        foreach (var companyPostViewModel in companyPostViewModels)
        {
            companyPostViewModel.Post = posts.First(c => c.Id == companyPostViewModel.Post.Id);
        }
        
        return companyPostViewModels;
    }
}