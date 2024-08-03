using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

public class HistoryActionDto : BaseDto
{
    [Required] 
    public required BaseId CompanyUser { get; set; }
    
    [Required] 
    public required BaseId ProjectTask { get; set; }
    
    [Required] 
    public required ActionState ActionState { get; set; }
}