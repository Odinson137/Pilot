using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class ProjectViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    public ICollection<TeamViewModel> Teams { get; set; } = new List<TeamViewModel>();

    public BaseDto Company { get; set; } = null!;
    
    public CompanyUserViewModel? CreatedBy { get; set; }
    
    public ProjectStatus ProjectStatus { get; set; }
}