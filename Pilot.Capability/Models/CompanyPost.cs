using System.ComponentModel.DataAnnotations;
using Pilot.Capability.Models.ModelHelpers;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Models;

public class CompanyPost : BaseModel, IAddCompanyUser
{
    public int? CompanyUserId { get; set; } // если пусто, значит вакансия может быть открыта

    public bool IsOpen { get; set; } = true;
    [Required] public Post Post { get; set; } = null!;
    
    [MaxLength(500)] public string? Description { get; set; }
    
    public void AddCompanyUser(int companyUserId)
    {
        CompanyUserId = companyUserId;
    }
}