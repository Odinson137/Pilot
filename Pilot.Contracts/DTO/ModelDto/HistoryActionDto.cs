using System.ComponentModel.DataAnnotations;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;

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