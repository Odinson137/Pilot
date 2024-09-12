using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyUserViewModel : BaseViewModel
{
    [Required] public CompanyViewModel Company { get; set; } = null!;

    public List<TeamViewModel> Team { get; set; } = [];

    public List<ProjectTaskViewModel> ProjectTasks { get; set; } = [];
    
    public CompanyRoleViewModel? CompanyRole { get; set; }
}