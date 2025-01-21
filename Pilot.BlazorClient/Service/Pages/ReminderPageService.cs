using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class ReminderPageService(IBaseModelService<ChatReminderViewModel> reminderService) 
    : BasePageService<ChatReminderViewModel>(reminderService), IReminderPageService
{
    private readonly IBaseModelService<ChatReminderViewModel> _reminderService = reminderService;

    public Task<ICollection<ChatReminderViewModel>> GetReminderAsync(int chatId)
    {
        return _reminderService.GetValuesAsync(c => c.ChatId, chatId);
    }
}