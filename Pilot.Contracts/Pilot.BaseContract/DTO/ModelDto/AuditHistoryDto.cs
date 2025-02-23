using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.AuditHistoryService)]
public class AuditHistoryDto : BaseDto
{
    public int UserId { get; set; }

    public int EntityId { get; set; }
    
    public ModelType EntityType { get; set; }

    [MaxLength(1000)] public string NewValue { get; set; } = null!;
    
    public ActionState ActionState { get; set; }
}