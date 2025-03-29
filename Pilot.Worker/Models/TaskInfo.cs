using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Worker.Models.ModelHelpers;

namespace Pilot.Worker.Models;

public class TaskInfo : BaseModel, IAddCompanyUser
{
    [Required] public ProjectTask ProjectTask { get; set; } = null!;

    [MaxLength(50)]
    public string? File { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public CompanyUser CreatedBy { get; set; } = null!;

    public TimeSpan TimeSpent { get; set; }
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CreatedBy = companyUser;
    }
}