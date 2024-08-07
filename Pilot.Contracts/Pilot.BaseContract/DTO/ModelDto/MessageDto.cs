﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

public class MessageDto : BaseDto
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }
    
    [Required] public BaseDto User { get; set; } = null!;
    
    public MessagePriority MessagePriority { get; set; }
    
    public ModelType? EntityType { get; set; }
    
    public int? EntityId { get; set; }
}