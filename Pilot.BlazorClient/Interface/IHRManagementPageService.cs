using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.HelperViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IHrManagementPageService
{
    Task<ICollection<PostViewModel>> GetPositionsAsync(int companyId);
    Task AddPositionAsync(PostViewModel position);
    Task UpdatePositionAsync(PostViewModel position);
    Task DeletePositionAsync(int positionId);
    Task<ICollection<CompanyPostViewModel>> GetPostsAsync(int companyId);
    Task AddPostAsync(CompanyPostViewModel post);
    Task UpdatePostAsync(CompanyPostViewModel post);
    Task DeletePostAsync(int postId);
    Task<ICollection<JobApplicationViewModel>> GetCompanyJobApplicationsAsync(int companyId);
    Task UpdateApplicationStatusAsync(JobApplicationViewModel application);
    Task<ICollection<SkillViewModel>> GetAvailableSkillsAsync();
}