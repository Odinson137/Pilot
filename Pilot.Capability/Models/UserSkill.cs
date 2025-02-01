using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Capability.Models;

public class UserSkill : BaseModel
{
    [Required] public Skill Skill { get; set; } = null!;

    [Required] public int UserId { get; set; }
    
    public int? ExperienceYears { get; set; }
    
    public SkillLevel SkillLevel { get; set; }
}