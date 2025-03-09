using MassTransit;
using Pilot.AuditHistory.Interface;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.SqrsControllerLibrary.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.AuditHistory.Consumers;

public class TaskUpdatedConsumer: IConsumer<UpdateCommandMessage<ProjectTaskDto>>
{
    private readonly IBaseRepository<Models.AuditHistory> _auditHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskUpdatedConsumer(IBaseRepository<Models.AuditHistory> auditHistoryRepository, IUnitOfWork unitOfWork)
    {
        _auditHistoryRepository = auditHistoryRepository;
        _unitOfWork = unitOfWork;
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
        await _auditHistoryRepository.AddValueToContextAsync(audit, context.CancellationToken);
        await _unitOfWork.SaveChangesAsync();
    }
}