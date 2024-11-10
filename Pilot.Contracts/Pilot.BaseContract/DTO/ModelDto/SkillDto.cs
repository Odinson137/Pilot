using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.CapabilityServer)]
public class SkillDto : BaseDto
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;
}