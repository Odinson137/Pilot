using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Worker.Models.ModelHelpers;

namespace Pilot.Worker.Models;

// TODO идельный пример того, что можно потом перенсти в другой серсис
public class HistoryAction : BaseModel, IAddCompanyUser
{
    [Required] public CompanyUser CompanyUser { get; set; } = null!;

    [Required] public ProjectTask ProjectTask { get; set; } = null!;

    [Required] [MaxLength(500)] public string LastValue { get; set; } = null!;
    
    [Required] public ActionState ActionState { get; set; }

    public void AddCompanyUser(CompanyUser companyUser)
    {
        CompanyUser = companyUser;
    }
}