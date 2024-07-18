using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

public class MessageDto : BaseDto
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }
    
    public MessagePriority MessagePriority { get; set; }
}