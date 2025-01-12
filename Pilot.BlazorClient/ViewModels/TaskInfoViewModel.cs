using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class TaskInfoViewModel : BaseViewModel
{
    [Required] public BaseViewModel ProjectTask { get; set; } = null!;

    public string? File { get; set; }
    
    public Byte[]? FileBytes { get; set; }

    [MaxLength(500)] public string? Description { get; set; }

    public CompanyUserViewModel? CreatedBy { get; set; }
}