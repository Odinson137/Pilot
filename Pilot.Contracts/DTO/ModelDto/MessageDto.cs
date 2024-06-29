using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class MessageDto : BaseModel
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public MessagePriority MessagePriority { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now; 
}