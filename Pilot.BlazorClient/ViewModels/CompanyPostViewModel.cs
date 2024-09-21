using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyPostViewModel : BaseViewModel
{
    public int? CompanyUserId { get; set; } 

    public bool IsOpen { get; set; }

    [Required] public PostViewModel Post { get; set; } = null!;
    
    [MaxLength(500)] public string? Description { get; set; }
}