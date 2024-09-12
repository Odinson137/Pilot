using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class TeamViewModel : BaseViewModel
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [Required] [MaxLength(500)] public string Description { get; set; } = null!;

    public List<CompanyUserViewModel> CompanyUsers { get; set; } = [];

    [Required] public ProjectViewModel ProjectViewModel { get; set; } = null!;
    
    public void AddCompanyUser(CompanyUserViewModel companyUserViewModel)
    {
        CompanyUsers.Add(companyUserViewModel);
    }
}