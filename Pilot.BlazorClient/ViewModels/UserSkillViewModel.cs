using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class UserSkillViewModel : BaseViewModel
{
    [Required] public SkillViewModel Skill { get; set; } = null!;

    [Required] [Range(1, int.MaxValue)] public int UserId { get; set; }
    
    public int? ExperienceYears { get; set; }
    
    public SkillLevel SkillLevel { get; set; }
}