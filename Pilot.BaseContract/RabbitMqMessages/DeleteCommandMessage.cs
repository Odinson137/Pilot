namespace Pilot.Contracts.RabbitMqMessages;

public record DeleteCommandMessage<TDto>(TDto Value, int UserId) : BaseCommandMessage<TDto>(Value, UserId);
