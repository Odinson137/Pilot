using AutoMapper;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Pilot.BlazorClient.Data;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;

namespace Pilot.BlazorClient.Service;

public class UserService : IUserService
{
    private readonly ProtectedSessionStorage _protectedSessionStore;
    private readonly IGateWayApiService _apiService;
    private readonly IMapper _mapper;
    
    public UserService(ProtectedSessionStorage protectedSessionStore, IGateWayApiService apiService, IMapper mapper)
    {
        _protectedSessionStore = protectedSessionStore;
        _apiService = apiService;
        _mapper = mapper;
    }

    public async Task<UserViewModel> GetCurrentUserAsync()
    {
        var storageResult = await _protectedSessionStore.GetAsync<UserViewModel>(ClientConstants.User);
        UserViewModel user;
        if (storageResult.Success)
        {
            user = storageResult.Value!;
            return user;
        }
        
        var userDto = await _apiService.SendGetMessage<UserDto>(Urls.CurrentUser);

        user = userDto.Map<UserViewModel>(_mapper);
        await _protectedSessionStore.SetAsync(ClientConstants.User, user);
        return user;
    }
}