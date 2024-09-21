using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Worker.Models.ModelHelpers;

namespace Pilot.Worker.Models;

public class ProjectTask : BaseModel, IAddCompanyUser
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] public Team Team { get; set; } = null!;

    public CompanyUser? CompanyUser { get; set; }

    [Required] public CompanyUser CreatedBy { get; set; } = null!;
    
    public List<TaskInfo> TaskInfos { get; set; } = [];

    public int? FileId { get; set; }
    
    public TaskStatus TaskStatus { get; set; }

    public void AddCompanyUser(CompanyUser companyUser)
    {
        CreatedBy = companyUser;
    }
}