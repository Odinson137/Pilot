using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Interface;

public interface ISkillPageService
{
    Task<ICollection<SkillViewModel>> GetAllSkillAsync();
    
    Task<UserViewModel> GetUserAsync();
    
    Task AddUserSkillAsync(UserSkillViewModel userSkillViewModel);
    
    Task UpdateUserSkillAsync(UserSkillViewModel userSkillViewModel);
    
    Task DeleteUserSkillAsync(int skillId);
    
    Task<ICollection<UserSkillViewModel>> GetAllUserSkillAsync(int userId);
}