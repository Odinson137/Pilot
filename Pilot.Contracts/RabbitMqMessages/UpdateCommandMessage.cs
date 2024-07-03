namespace Pilot.Contracts.RabbitMqMessages;

public record UpdateCommandMessage<TDto>(TDto Value, string UserId) : BaseCommandMessage<TDto>(Value, UserId);
