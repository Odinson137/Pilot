using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

public class MessageDto : BaseDto
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }
    
    public MessagePriority MessagePriority { get; set; }
    
    // TODO будет неточность, если вдруг я захочу поменять название для моих моделей, то здесь будет несостыковка.
    // Придется делать миграцию
    // Или думать, как сделать это красиво.
    // Первый вариант - сделать через enum
    public string? EntityType { get; set; }
    
    public int? EntityId { get; set; }
}