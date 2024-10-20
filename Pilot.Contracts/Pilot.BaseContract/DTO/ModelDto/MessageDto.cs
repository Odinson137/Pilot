using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.MessengerServer)]
public class MessageDto : BaseDto
{
    [MaxLength(1000)] public string? Text { get; set; }

    [Required] public int UserId { get; set; }

    [Required] public BaseDto Chat { get; set; } = null!;
}