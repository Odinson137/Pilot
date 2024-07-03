namespace Pilot.Contracts.RabbitMqMessages;

public record DeleteCommandMessage<TDto>(TDto Value, string UserId) : BaseCommandMessage<TDto>(Value, UserId);
