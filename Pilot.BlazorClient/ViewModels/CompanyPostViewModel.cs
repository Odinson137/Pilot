using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyPostViewModel : BaseViewModel
{
    [Required] public PostViewModel Post { get; set; } = null!;
    
    public ICollection<JobApplicationViewModel> Applications { get; set; } = [];
    
    [Required] public bool IsOpen { get; set; } = true;
    
    [MaxLength(500)] public string? AdditionalRequirements { get; set; }
}