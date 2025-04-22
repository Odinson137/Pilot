using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyPostViewModel : BaseViewModel
{
    [Required] public PostViewModel Post { get; set; } = null!;
    
    [Required]
    public int? PostId
    {
        get => Post.Id;

        set => Post = new PostViewModel { Id = value ?? 0 };
    }
    
    public ICollection<JobApplicationViewModel> Applications { get; set; } = [];
    
    [Required] public bool IsOpen { get; set; } = true;
    
    [MaxLength(500)] public string? AdditionalRequirements { get; set; }
}