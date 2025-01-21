using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IReminderPageService : IBasePageService<ChatReminderViewModel>
{
    Task<ICollection<ChatReminderViewModel>> GetReminderAsync(int chatId);
}