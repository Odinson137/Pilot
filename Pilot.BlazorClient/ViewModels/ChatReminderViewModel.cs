using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class ChatReminderViewModel : BaseViewModel
{
    public int ChatId { get; set; }
    
    public TimeOnly Time { get; set; }
    public List<DayOfWeek> DayOfWeeks { get; set; } = [];
    
    [MaxLength(500)]
    public string? Message { get; set; }

    public int CreatedBy { get; set; }

    public ReminderStatus ReminderStatus { get; set; } = ReminderStatus.Work;
}