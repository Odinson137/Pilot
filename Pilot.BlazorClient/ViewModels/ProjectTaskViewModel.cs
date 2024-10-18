using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;
using TaskStatus = Pilot.Contracts.Data.Enums.TaskStatus;

namespace Pilot.BlazorClient.ViewModels;

public class ProjectTaskViewModel : BaseViewModel
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] public TeamViewModel TeamViewModel { get; set; } = null!;

    public CompanyUserViewModel? CompanyUser { get; set; }

    [Required] public CompanyUserViewModel CreatedBy { get; set; } = null!;
    
    public ICollection<TaskInfoViewModel> TaskInfos { get; set; } = [];

    public int? FileId { get; set; }
    
    public TaskStatus TaskStatus { get; set; }
    
    public TaskPriority Priority { get; set; }
}