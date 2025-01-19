using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface ICompanyPostPageService : IBaseModelService<CompanyPostViewModel>
{
    Task<ICollection<CompanyPostViewModel>> GetVacanciesAsync(int take, int skip, string? search = null);
    
    Task<CompanyPostViewModel> GetVacancyAsync(int vacancyId);

    Task<ICollection<SkillViewModel>> GetPostSkillsAsync(PostViewModel post);

    Task<CompanyViewModel> GetCompanyAsync(int companyId);
    
    Task SubmitApplicationAsync(int vacancyId, string letter);
}