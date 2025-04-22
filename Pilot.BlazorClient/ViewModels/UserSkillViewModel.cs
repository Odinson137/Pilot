using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class UserSkillViewModel : BaseViewModel
{
    public SkillViewModel? Skill { get; set; }

    [Required]
    public int? SkillId
    {
        get => Skill?.Id;

        set => Skill = new SkillViewModel { Id = value ?? 0 };
    }

    public int UserId { get; set; }

    [Range(1, 50)] public int? ExperienceYears { get; set; } = 1;

    public SkillLevel SkillLevel { get; set; }
}