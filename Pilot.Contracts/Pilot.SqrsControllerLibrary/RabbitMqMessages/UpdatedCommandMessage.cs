namespace Pilot.SqrsControllerLibrary.RabbitMqMessages;

public record UpdatedCommandMessage<TDto>(TDto Value, int UserId, Guid SagaId) : BaseCommandMessage<TDto>(Value, UserId);