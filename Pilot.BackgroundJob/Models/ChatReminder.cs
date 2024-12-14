using Pilot.Contracts.Base;

namespace Pilot.BackgroundJob.Models;

public class ChatReminder : BaseModel
{
    public int ChatId { get; set; }
    
    public TimeOnly Time { get; set; }

    public ICollection<DayOfWeek> DayOfWeeks { get; set; } = [];
    
    public string? Title { get; set; }
    
    public string? Message { get; set; }

    public int CreateBy { get; set; }
}