using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class HistoryActionDto : BaseDto
{
    public required BaseId CompanyUser { get; set; }
    public required BaseId ProjectTask { get; set; }
    public required ActionState ActionState { get; set; }
    public DateTime Timestamp { get; set; }
}