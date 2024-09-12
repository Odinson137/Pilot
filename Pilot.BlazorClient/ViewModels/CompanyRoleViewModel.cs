using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyRoleViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Title { get; init; } = null!;
    
    public List<CompanyUserViewModel> CompanyUsers { get; set; } = [];

    public List<CompanyViewModel> Companies { get; set; } = [];
    
    public bool IsBaseRole { get; init; }
}