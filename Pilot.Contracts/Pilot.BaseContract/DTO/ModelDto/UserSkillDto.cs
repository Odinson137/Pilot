using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.CapabilityServer)]
public class UserSkillDto : BaseDto
{
    [Required] public BaseDto Skill { get; set; } = null!;

    [Required] [Range(1, int.MaxValue)] public int UserId { get; set; }
    
    public int? ExperienceYears { get; set; }
    
    public SkillLevel SkillLevel { get; set; }
}