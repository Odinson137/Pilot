using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Worker.Models.ModelHelpers;

namespace Pilot.Worker.Models;

public class TaskInfo : BaseModel, IAddCompanyUser
{
    [Required] public ProjectTask ProjectTask { get; set; } = null!;

    public int? FileId { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public CompanyUser CreatedBy { get; set; } = null!;
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CreatedBy = companyUser;
    }
}