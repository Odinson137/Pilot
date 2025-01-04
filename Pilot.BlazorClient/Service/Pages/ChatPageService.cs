using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class ChatPageService(IGateWayApiService apiService, IUserService userService, IMapper mapper) 
    : BaseModelService<ChatDto, ChatViewModel>(apiService, mapper), IChatPageService
{
    private readonly IMapper _mapper = mapper;
    private readonly IGateWayApiService _apiService = apiService;

    public async Task<ICollection<ChatViewModel>> GetUserChatsAsync()
    {
        var chats = await _apiService.SendGetMessages<ChatDto, ChatViewModel>(Urls.UserChats);
        return chats;
    }

    public async Task<ChatViewModel> GetChatAsync(int chatId)
    {
        var chat = await _apiService.SendGetMessage<ChatDto, ChatViewModel>(chatId);
        return chat;
    }

    public async Task<ICollection<MessageViewModel>> GetMessagesAsync(int chatId, int start, int end)
    {
        var filter = new BaseFilter(start, end);
        var values = await _apiService.SendGetMessages<MessageDto, MessageViewModel>($"{Urls.ChatMessages}/{chatId}", filter);
        return values;
    }

    public async Task<ICollection<InfoMessageViewModel>> GetInfoMessagesAsync(int start, int end)
    {
        var values = await _apiService.SendGetMessages<InfoMessageDto, InfoMessageViewModel>();
        return values;
    }

    public async Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<ChatMemberViewModel> chatMemberViewModels)
    {
        var ids = chatMemberViewModels.Select(c => c.Id).ToList();
        var filter = new BaseFilter(ids);
        var filledChatMemberViewModels = await _apiService.SendGetMessages<ChatMemberDto, ChatMemberViewModel>(filter: filter);
        var userFilter = new BaseFilter(filledChatMemberViewModels.Select(c => c.UserId).ToList());
        var userViewModels = await _apiService.SendGetMessages<UserDto, UserViewModel>(filter: userFilter);
        return userViewModels;
    }

    public async Task<ICollection<UserViewModel>> GetAllEmployeesAsync()
    {
        var user = await userService.GetCurrentUserAsync();
        var companyUser = await _apiService.SendGetMessage<CompanyUserDto, CompanyUserViewModel>(user.Id);
        var company = await _apiService.SendGetMessage<CompanyDto, CompanyViewModel>(companyUser.Company.Id);
        var filter = new BaseFilter(company.CompanyUsers.Select(c => c.Id).ToArray());
        var users = await _apiService.SendGetMessages<UserDto, UserViewModel>(filter: filter);
        return users;
    }

    public async Task CreateChatAsync(ChatViewModel chat)
    {
        var taskDto = _mapper.Map<ChatDto>(chat);
        await _apiService.SendPostMessage(null, message: taskDto);
    }
}