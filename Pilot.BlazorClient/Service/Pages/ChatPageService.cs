using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.HelperViewModels;
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
    IBaseModelService<CompanyUserViewModel> companyUserService,
    IBaseModelService<UserViewModel> userBaseService,
    IMessengerService messengerService,
    IUserService userService) : BasePageService<ChatViewModel>(chatService, messengerService), IChatPageService
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

    // пусть юзеры из вне не имеют возможности добавлять какого-то в чат, в юзеры в компании могу
    public async Task<ICollection<MultySelectViewModel<UserViewModel>>> GetAllEmployeesAsync(ChatViewModel chat)
    {
        var user = await userService.GetCurrentUserAsync();
        var chatMember = await chatMemberService.GetValuesAsync(chat.ChatMembers.Select(c => c.Id).ToList());
        var memberIds = chatMember.Select(c => c.UserId).ToList();
        
        var companyUserHas = await companyUserService.GetValuesAsync(c => c.UserId, user.Id);
        if (companyUserHas.Count > 0)
        {
            var company = await companyService.GetValueAsync(companyUserHas.First().Company.Id);
            var companyUsers = 
                await userBaseService.GetValuesAsync(company.CompanyUsers.Select(c => c.Id).ToArray());
            
            var multySelectCompanyUsers = MultySelectViewModel<UserViewModel>.GetList(companyUsers);
            foreach (var select in multySelectCompanyUsers)
            {
                if (memberIds.Contains(select.Value.Id))
                {
                    select.IsSelected = true;
                }
            }
            return multySelectCompanyUsers;
        }

        var users = await userBaseService.GetValuesAsync(memberIds);
        var multySelect = MultySelectViewModel<UserViewModel>.GetList(users);
        return multySelect;
    }
}