using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class TaskInfoViewModel : BaseViewModel
{
    [Required] public ProjectTaskViewModel ProjectTaskViewModel { get; set; } = null!;

    public int? FileId { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public CompanyUserViewModel CreatedBy { get; set; } = null!;
    
    public void AddCompanyUser(CompanyUserViewModel companyUserViewModel)
    {
        CreatedBy = companyUserViewModel;
    }
}