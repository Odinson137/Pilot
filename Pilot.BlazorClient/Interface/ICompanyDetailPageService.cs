using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface ICompanyDetailPageService
{
    Task<CompanyViewModel> GetCompanyAsync(int id);

    Task<ICollection<ProjectViewModel>> GetProjectsAsync(ICollection<ProjectViewModel> projectsIds);

    Task<ICollection<CompanyPostViewModel>> GetOpenCompanyPostAsync(int companyId);
}