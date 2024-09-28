using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class SkillViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;
}