using System.Security.Authentication;
using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;

namespace Pilot.BlazorClient.Service.Pages;

public class SkillPageService(
    IUserService userBaseService,
    IBaseModelService<SkillViewModel> skillService,
    IBaseModelService<UserSkillViewModel> userSkillService,
    ILogger<SkillPageService> logger) 
    : ISkillPageService
{
    public Task<ICollection<SkillViewModel>> GetAllSkillAsync()
    {
        return skillService.GetValuesAsync();
    }

    public Task<UserViewModel> GetUserAsync()
    {
        return userBaseService.GetCurrentUserAsync();
    }
    
    public async Task<ICollection<UserSkillViewModel>> GetAllUserSkillAsync(int userId)
    {
        var userSkills = await userSkillService.GetValuesAsync(c => c.UserId, userId);
        var needSkill = await skillService.GetValuesAsync(userSkills.Select(c => c.Skill.Id).ToList());
        foreach (var userSkill in userSkills)
            userSkill.Skill = needSkill.Single(s => s.Id == userSkill.Skill.Id);

        return userSkills;
    }
    
    public async Task UpdateUserSkillAsync(UserSkillViewModel userSkillViewModel)
    {
        await userSkillService.UpdateValueAsync(userSkillViewModel);
    }

    public async Task DeleteUserSkillAsync(int skillId)
    {
        await userSkillService.DeleteValueAsync(skillId);
    }

    public async Task AddUserSkillAsync(UserSkillViewModel userSkillViewModel)
    {
        await userSkillService.CreateValueAsync(userSkillViewModel);
    }
}