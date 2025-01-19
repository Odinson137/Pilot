using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class PostViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] [Range(1, int.MaxValue)] public int CompanyId { get; set; }

    public ICollection<BaseViewModel> Skills { get; set; } = [];

    public ICollection<BaseViewModel> CompanyPosts { get; set; } = [];
}