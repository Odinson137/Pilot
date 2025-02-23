using MassTransit;
using Pilot.AuditHistory.Interface;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.AuditHistory.Consumers;

public class TaskUpdatedConsumer: IConsumer<UpdateCommandMessage<ProjectTaskDto>>
{
    private readonly IClickHouseService _clickHouseService;

    public TaskUpdatedConsumer(IClickHouseService clickHouseService)
    {
        _clickHouseService = clickHouseService;
    }

    public async Task Consume(ConsumeContext<UpdateCommandMessage<ProjectTaskDto>> context)
    {
        var audit = new Models.AuditHistory
        {
            UserId = context.Message.UserId,
            EntityId = context.Message.Value.Id,
            EntityType = ModelType.ProjectTask,
            NewValue = context.Message.Value.ToJson(),
            ActionState = ActionState.Update
        };
        await _clickHouseService.InsertAuditLogAsync(audit, context.CancellationToken);
    }
}