using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface ICompanyPageService : IBasePageService<CompanyViewModel>
{
    Task<ICollection<ProjectViewModel>> GetProjectsAsync(ICollection<ProjectViewModel> projectsIds);

    Task<ICollection<CompanyPostViewModel>> GetOpenCompanyPostAsync(int companyId);
}