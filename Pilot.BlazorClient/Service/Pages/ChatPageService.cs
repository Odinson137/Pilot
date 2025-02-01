using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class ChatPageService(
    IBaseModelService<ChatViewModel> chatService,
    IBaseModelService<CompanyViewModel> companyService,
    IBaseModelService<ChatMemberViewModel> chatMemberService,
    IBaseModelService<InfoMessageViewModel> infoMessageViewService,
    IBaseModelService<UserViewModel> userBaseService,
    IUserService userService) : BasePageService<ChatViewModel>(chatService), IChatPageService
{
    private readonly IBaseModelService<ChatViewModel> _chatService = chatService;

    public async Task<ICollection<ChatViewModel>> GetUserChatsAsync()
    {
        var chats = await _chatService.Client.SendGetMessages<ChatDto, ChatViewModel>(Urls.UserChats);
        return chats;
    }

    public async Task<ICollection<MessageViewModel>> GetMessagesAsync(int chatId, int start, int end)
    {
        var filter = new BaseFilter(0, int.MaxValue);
        var values =
            await _chatService.Client.SendGetMessages<MessageDto, MessageViewModel>($"{Urls.ChatMessages}/{chatId}",
                filter);
        return values;
    }

    public async Task<ICollection<InfoMessageViewModel>> GetInfoMessagesAsync(int start, int end)
    {
        return await infoMessageViewService.GetValuesAsync();
    }

    public async Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<ChatMemberViewModel> chatMemberViewModels)
    {
        var filledChatMemberViewModels =
            await chatMemberService.GetValuesAsync(chatMemberViewModels.Select(c => c.Id).ToList());
        var userViewModels = 
            await userBaseService.GetValuesAsync(filledChatMemberViewModels.Select(c => c.UserId).ToList());
        return userViewModels;
    }

    public async Task<ICollection<UserViewModel>> GetAllEmployeesAsync()
    {
        var user = await userService.GetCurrentUserAsync();
        var companyUser = await companyService.Client.SendGetMessage<CompanyUserDto, CompanyUserViewModel>(user.Id);
        var company = await companyService.Client.SendGetMessage<CompanyDto, CompanyViewModel>(companyUser.Company.Id);
        var users = 
            await userBaseService.GetValuesAsync(company.CompanyUsers.Select(c => c.Id).ToArray());
        return users;
    }
}