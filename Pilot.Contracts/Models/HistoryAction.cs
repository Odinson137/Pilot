using System.ComponentModel.DataAnnotations;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class HistoryAction : BaseModel
{
    [Required] public required CompanyUser CompanyUser { get; set; }
    [Required] public required ProjectTask ProjectTask { get; set; }
    [Required] public required ActionState ActionState { get; set; }
    [Required] public DateTime Timestamp { get; set; } = DateTime.Now;
}