using MassTransit;

namespace Pilot.SqrsControllerLibrary.RabbitMqMessages;

public class UpdateSagaState<TDto> : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public TDto? OldValue { get; set; }

    public TDto? NewValue { get; set; }
    
    public int UserId { get; set; }
    
    public string? CurrentState { get; set; }
}