using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class ProjectTaskViewModel : BaseViewModel
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] public TeamViewModel TeamViewModel { get; set; } = null!;

    public CompanyUserViewModel? CompanyUser { get; set; }

    [Required] public CompanyUserViewModel CreatedBy { get; set; } = null!;
    
    public List<TaskInfoViewModel> TaskInfos { get; set; } = [];

    public int? FileId { get; set; }
    
    public TaskStatus TaskStatus { get; set; }

    public void AddCompanyUser(CompanyUserViewModel companyUserViewModel)
    {
        CreatedBy = companyUserViewModel;
    }
}