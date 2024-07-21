using System.ComponentModel.DataAnnotations;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class HistoryAction : BaseModel
{
    [Required] public CompanyUser CompanyUser { get; set; } = null!;

    [Required] public ProjectTask ProjectTask { get; set; } = null!;
    
    [Required] public ActionState ActionState { get; set; }
}