using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.MessengerServer)]
public class InfoMessageDto : BaseDto
{
    public string Title => $"Успешное {Action}";

    private string Action => MessagePriority switch
    {
        MessageInfo.Create => "создание",
        MessageInfo.Update => "обновление",
        MessageInfo.Default => "удаление",
        _ => string.Empty
    };

    [MaxLength(500)] public string Description => $"Успешное {Action} сущности {EntityType}";

    [Required] public MessageInfo MessagePriority { get; set; }

    public ModelType? EntityType { get; set; }

    public int? EntityId { get; set; }
}