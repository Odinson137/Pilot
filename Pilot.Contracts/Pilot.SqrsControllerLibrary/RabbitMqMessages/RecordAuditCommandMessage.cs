namespace Pilot.SqrsControllerLibrary.RabbitMqMessages;

public record RecordAuditCommandMessage<TDto>(TDto Value, int UserId, Guid SagaId) : BaseCommandMessage<TDto>(Value, UserId);