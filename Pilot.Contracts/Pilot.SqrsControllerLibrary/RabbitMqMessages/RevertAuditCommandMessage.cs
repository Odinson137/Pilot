namespace Pilot.SqrsControllerLibrary.RabbitMqMessages;

public record RevertAuditCommandMessage<TDto>(TDto Value, Guid SagaId) : BaseCommandMessage<TDto>(Value, 0);