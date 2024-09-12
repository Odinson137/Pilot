using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class TaskInfoDto : BaseDto
{
    [Required] public BaseDto ProjectTask { get; set; } = null!;

    public int? FileId { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public BaseDto CreatedBy { get; set; } = null!;
}