using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;

namespace Pilot.BlazorClient.Service.Pages;

public class UserPageService(IGateWayApiService apiService, IUserService userService, ILogger<UserPageService> logger, IMapper mapper) : IUserPageService
{
    public async Task Registration(RegistrationUserViewModel registrationUser)
    {
        var client = await apiService.GetClientAsync<AuthorizationUserViewModel>();
        var response = await client.PostAsJsonAsync("api/User/Registration", registrationUser);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
    
    public async Task<AuthUserViewModel> Authorization(AuthorizationUserViewModel authorizationUser)
    {
        var client = await apiService.GetClientAsync<AuthorizationUserViewModel>();
        var response = await client.PostAsJsonAsync("api/User/Authorization", authorizationUser);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var auth = await response.Content.ReadFromJsonAsync<AuthUserViewModel>();

        if (auth == null)
        {
            throw new Exception("Ошибка конвертации файла");
        }
        
        return auth;
    }
    
    public async Task<UserViewModel> GetUserAsync()
    {
        var user = await userService.GetCurrentUserAsync();
        return user;
    }

    public async Task<UserViewModel> GetAnotherUserAsync(int userId)
    {
        var userDto = await apiService.SendGetMessage<UserDto>(userId.ToString());

        var user = userDto.Map<UserViewModel>(mapper);
        return user;
    }

    public async Task<ICollection<UserSkillViewModel>> GetUserSkillAsync(int userId)
    {
        var userSkillsDto = await apiService.SendGetMessages<UserSkillDto, UserSkillViewModel>($"{Urls.UserSkills}/{userId}");
        var userSkills = userSkillsDto.Map<ICollection<UserSkillViewModel>>(mapper);

        var filter = new BaseFilter(userSkills.Select(c => c.Skill.Id).ToList());
        var skillsDto = await apiService.SendGetMessages<SkillDto, SkillViewModel>(filter: filter);

        foreach (var userSkill in userSkillsDto)
        {
            userSkill.Skill = skillsDto.First(c => c.Id == userSkill.Skill.Id);
        }
        return userSkillsDto;
    }
}