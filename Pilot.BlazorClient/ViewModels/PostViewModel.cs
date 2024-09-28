using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class PostViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    [Required] [Range(1, int.MaxValue)] public int CompanyId { get; set; }

    public ICollection<SkillViewModel> Skills { get; set; } = [];
}