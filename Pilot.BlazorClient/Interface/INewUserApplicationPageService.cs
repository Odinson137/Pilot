using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface INewUserApplicationPageService
{
    Task<ICollection<JobApplicationViewModel>> GetUserJobApplicationsAsync();
    
    Task DeleteApplicationAsync(int applicationId, Action<InfoMessageViewModel>? action = null);
}