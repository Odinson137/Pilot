using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

[FromService(ServiceName.AuditHistoryService)]
public class AuditHistoryViewModel : BaseViewModel
{
    public int UserId { get; set; }

    public int EntityId { get; set; }
    
    public ModelType EntityType { get; set; }

    [MaxLength(1000)] public string NewValue { get; set; } = null!;
    
    public ActionState ActionState { get; set; }
}