using System.ComponentModel.DataAnnotations;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class CompanyUserViewModel : BaseViewModel
{
    [Required] public CompanyViewModel Company { get; set; } = null!;

    public int PostId { get; set; }

    public ICollection<TeamViewModel> Teams { get; set; } = [];

    public ICollection<ProjectTaskViewModel> ProjectTasks { get; set; } = [];
    
    public UserViewModel? User { get; set; }
    
    public Permission Permissions { get; set; }
}