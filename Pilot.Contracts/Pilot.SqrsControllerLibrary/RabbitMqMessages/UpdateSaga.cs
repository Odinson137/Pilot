using MassTransit;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.SqrsControllerLibrary.RabbitMqMessages;

public class UpdateSaga<TDto> : MassTransitStateMachine<UpdateSagaState<TDto>>
    where TDto : BaseDto
{
    public State Updating { get; private set; }
    public State Auditing { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    public Event<UpdateCommand<TDto>> UpdateRequested { get; private set; }
    public Event<UpdatedCommandMessage<TDto>> UpdatedEvent { get; private set; }
    public Event<UpdateFailedCommandMessage<TDto>> UpdateFailedEvent { get; private set; }
    public Event<RecordAuditCommandMessage<TDto>> AuditRecorded { get; private set; }

    public UpdateSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => UpdateRequested, x => x.CorrelateById(context => context.Message.SagaId));
        Event(() => UpdatedEvent, x => x.CorrelateById(context => context.Message.SagaId));
        Event(() => UpdateFailedEvent, x => x.CorrelateById(context => context.Message.SagaId));
        Event(() => AuditRecorded, x => x.CorrelateById(context => context.Message.SagaId));

        Initially(
            When(UpdateRequested)
                .Then(context =>
                {
                    context.Saga.NewValue = context.Message.ValueDto;
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.CorrelationId = context.Message.SagaId;
                })
                .Publish(context =>
                    new RecordAuditCommandMessage<TDto>(
                        context.Saga.NewValue, 
                        context.Saga.UserId, 
                        context.Saga.CorrelationId))
                .TransitionTo(Updating));

        During(Updating,
            When(UpdatedEvent)
                .Then(context =>
                {
                    context.Saga.OldValue = context.Message.Value;
                })
                .TransitionTo(Auditing),
            When(UpdateFailedEvent)
                .Publish(context =>
                    new RevertAuditCommandMessage<TDto>(
                        context.Saga.NewValue, 
                        context.Saga.CorrelationId))
                .TransitionTo(Failed));

        During(Auditing,
            When(AuditRecorded)
                .TransitionTo(Completed));
    }
}