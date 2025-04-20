using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyViewModel : BaseViewModel
{
    [Required] [MaxLength(50)] public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    public ICollection<ProjectViewModel> Projects { get; set; } = [];

    public ICollection<CompanyRoleViewModel> CompanyRoles { get; set; } = [];
    
    public ICollection<CompanyUserViewModel> CompanyUsers { get; set; } = [];

    public BaseViewModel? CreatedBy { get; set; }

    public string? Logo { get; set; }

    public List<string> InsideImages { get; set; } = [];
}