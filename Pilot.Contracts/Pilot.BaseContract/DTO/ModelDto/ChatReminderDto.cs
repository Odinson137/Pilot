using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.BackgroundJobService)]
public class ChatReminderDto : BaseDto
{
    public int ChatId { get; set; }
    
    public TimeOnly Time { get; set; }

    public ICollection<DayOfWeek> DayOfWeeks { get; set; } = [];
    
    [MaxLength(500)]
    public string? Message { get; set; }

    public int CreatedBy { get; set; }

    public ReminderStatus ReminderStatus { get; set; } = ReminderStatus.Work;
}