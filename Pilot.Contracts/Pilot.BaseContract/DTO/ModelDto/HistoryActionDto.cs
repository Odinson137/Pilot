using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class HistoryActionDto : BaseDto
{
    [Required] public required BaseDto CompanyUser { get; set; }

    [Required] public required BaseDto ProjectTask { get; set; }

    [Required] public required ActionState ActionState { get; set; }
}