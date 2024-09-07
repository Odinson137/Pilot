using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Receiver.Models.ModelHelpers;

namespace Pilot.Receiver.Models;

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