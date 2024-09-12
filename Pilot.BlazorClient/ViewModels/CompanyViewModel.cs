using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyViewModel : BaseViewModel
{
    [Required] [MaxLength(50)] public string Title { get; init; } = null!;

    [MaxLength(500)] public string? Description { get; init; }

    public List<ProjectViewModel> Projects { get; set; } = [];

    public List<CompanyRoleViewModel> CompanyRoles { get; set; } = [];
    
    public string? LogoId { get; set; }

    public List<string> InsideImages { get; set; } = [];
}