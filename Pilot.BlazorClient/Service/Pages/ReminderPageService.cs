using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Service.Pages;

public class ReminderPageService(IBaseModelService<ChatReminderViewModel> reminderService, IMessengerService messengerService) 
    : BasePageService<ChatReminderViewModel>(reminderService, messengerService), IReminderPageService
{
    private readonly IBaseModelService<ChatReminderViewModel> _reminderService = reminderService;

    public Task<ICollection<ChatReminderViewModel>> GetReminderAsync(int chatId)
    {
        return _reminderService.GetValuesAsync(c => c.ChatId, chatId);
    }
}