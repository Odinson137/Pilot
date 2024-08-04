using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models.ModelHelpers;

namespace Pilot.Contracts.Models;

public class ProjectTask : BaseModel, IAddCompanyUser
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public Project Project { get; set; } = null!;
    
    [Required] public CompanyUser CompanyUser { get; set; } = null!;
     
    public File? File { get; set; }
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CompanyUser = companyUser;
    }
}