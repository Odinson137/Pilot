namespace Pilot.SqrsControllerLibrary.RabbitMqMessages;

public record UpdateFailedCommandMessage<TDto>(TDto Value, int UserId, Guid SagaId) : BaseCommandMessage<TDto>(Value, UserId);