using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.ViewModels;

public class SkillViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;
}