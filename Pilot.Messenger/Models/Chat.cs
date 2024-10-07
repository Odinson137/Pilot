using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Messenger.Models;

public class Chat : BaseModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] public int UserId { get; set; }

    [Required] public MessageInfo MessagePriority { get; set; }

    public ModelType? EntityType { get; set; }

    public int? EntityId { get; set; }
}