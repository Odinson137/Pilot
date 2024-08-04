using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Models.ModelHelpers;

namespace Pilot.Contracts.Models;

public class HistoryAction : BaseModel, IAddCompanyUser
{
    [Required] public CompanyUser CompanyUser { get; set; } = null!;

    [Required] public ProjectTask ProjectTask { get; set; } = null!;
    
    [Required] public ActionState ActionState { get; set; }
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CompanyUser = companyUser;
    }
}