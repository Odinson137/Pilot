using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class TaskInfoViewModel : BaseViewModel
{
    public int? FileId { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public CompanyUserViewModel CreatedBy { get; set; } = null!;
}