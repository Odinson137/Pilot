using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IChatPageService : IBaseModelService<ChatViewModel>
{
    Task<ICollection<ChatViewModel>> GetUserChatsAsync();
    
    Task<ICollection<MessageViewModel>> GetMessagesAsync(int chatId, int start, int end);
    
    Task<ICollection<InfoMessageViewModel>> GetInfoMessagesAsync(int start, int end); // системные уведомления для пользователя
    
    Task<ICollection<UserViewModel>> GetUsersAsync(ICollection<ChatMemberViewModel> chatMemberViewModels);
    
    Task<ICollection<UserViewModel>> GetAllEmployeesAsync();
}
