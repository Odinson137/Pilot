using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.MessengerServer)]
public class InfoMessageDto : BaseDto
{
    public string Title => $"Успешное {Action}";

    private string Action
    {
        get
        {
            if ((MessagePriority & MessageInfo.Create) == MessageInfo.Create)
                return "создание";
            if ((MessagePriority & MessageInfo.Update) == MessageInfo.Update)
                return "обновление";
            if ((MessagePriority & MessageInfo.Delete) == MessageInfo.Delete)
                return "удаление";
            return string.Empty;
        }
    }

    [MaxLength(500)] public string Description => $"Успешное {Action} сущности {EntityType}";
    
    [Required] public MessageInfo MessagePriority { get; set; }

    public ModelType? EntityType { get; set; }

    public int? EntityId { get; set; }
}