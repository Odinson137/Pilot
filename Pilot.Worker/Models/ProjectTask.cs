using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Worker.Models.ModelHelpers;
using TaskStatus = Pilot.Contracts.Data.Enums.TaskStatus;

namespace Pilot.Worker.Models;

public class ProjectTask : BaseModel, IAddCompanyUser
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] public Team Team { get; set; } = null!;

    [Required] public CompanyUser CompanyUser { get; set; } = null!;

    [Required] public CompanyUser CreatedBy { get; set; } = null!;
    
    public List<TaskInfo> TaskInfos { get; set; } = [];

    public string? File { get; set; }
    
    public TaskStatus TaskStatus { get; set; }
    
    public TaskPriority Priority { get; set; }

    public TimeSpan EstimatedTime { get; set; }
    
    public TimeSpan TimeSpent { get; set; }
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CreatedBy = companyUser;
        CompanyUser = companyUser;
    }
}