using System.ComponentModel.DataAnnotations;
using Pilot.BlazorClient.ViewModels.UserViewModels;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyUserViewModel : BaseViewModel
{
    [Required] public CompanyViewModel Company { get; set; } = null!;

    public ICollection<TeamViewModel> Teams { get; set; } = [];

    public ICollection<ProjectTaskViewModel> ProjectTasks { get; set; } = [];
    
    public UserViewModel? User { get; set; }
}