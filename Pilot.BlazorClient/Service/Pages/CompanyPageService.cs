using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class CompanyPageService(
    IBaseModelService<ProjectViewModel> projectService,
    IBaseModelService<CompanyViewModel> companyService,
    IBaseModelService<PostViewModel> postService) : BasePageService<CompanyViewModel>(companyService), ICompanyPageService
{
    private readonly IBaseModelService<CompanyViewModel> _companyService = companyService;

    public async Task<ICollection<ProjectViewModel>> GetProjectsAsync(ICollection<ProjectViewModel> projectsIds)
    {
        var idArray = projectsIds.Select(c => c.Id).ToList();
        var projectViewModels =
            await projectService.GetValuesAsync(idArray);
        return projectViewModels;
    }
    
    public async Task<ICollection<CompanyPostViewModel>> GetOpenCompanyPostAsync(int companyId)
    {
        var filter = new BaseFilter(companyId);
        var companyPostViewModels = await _companyService.Client.SendGetMessages<CompanyPostDto, CompanyPostViewModel>(Urls.OpenCompanyPost, filter);

        var posts = 
            await postService.GetValuesAsync(companyPostViewModels.Select(c => c.Post.Id).ToArray());
        
        foreach (var companyPostViewModel in companyPostViewModels)
            companyPostViewModel.Post = posts.First(c => c.Id == companyPostViewModel.Post.Id);
        
        return companyPostViewModels;
    }
}