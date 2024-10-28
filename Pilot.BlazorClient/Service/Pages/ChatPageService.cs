using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class ChatPageService(IGateWayApiService apiService) : IChatPageService
{
    public async Task<ICollection<ChatViewModel>> GetUserChatsAsync()
    {
        var chats = await apiService.SendGetMessages<ChatDto, ChatViewModel>(Urls.UserChats);
        return chats;
    }

    public Task<ICollection<ChatMemberViewModel>> GetChatMembersAsync(ICollection<int> chatMemberIds)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<MessageViewModel>> GetMessagesAsync(int chatId, int start, int end)
    {
        var values = await apiService.SendGetMessages<MessageDto, MessageViewModel>($"{Urls.ChatMessages}/{chatId}");
        return values;
    }

    public Task<ICollection<InfoMessageViewModel>> GetInfoMessagesAsync(int start, int end)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<InfoMessageViewModel>> GetInfoMessagesAsync(ICollection<int> messagesIds, int start, int end)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<ChatMemberViewModel> chatMemberViewModels)
    {
        var ids = chatMemberViewModels.Select(c => c.Id).ToArray();
        var filter = new BaseFilter(ids);
        var userViewModels = await apiService.SendGetMessages<UserDto, UserViewModel>(filter: filter);
        return userViewModels;
    }
}