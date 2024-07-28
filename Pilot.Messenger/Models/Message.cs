using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Messenger.Models;

public class Message : BaseModel
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }
    
    public MessagePriority MessagePriority { get; set; }
    
    public string? EntityType { get; set; }
    
    public int? EntityId { get; set; }
}