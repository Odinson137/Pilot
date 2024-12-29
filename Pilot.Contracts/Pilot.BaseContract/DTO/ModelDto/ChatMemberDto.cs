using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.MessengerServer)]
public class ChatMemberDto : BaseDto
{
    [Required] public BaseDto Chat { get; set; } = null!;

    [Required] public int UserId { get; set; }
}