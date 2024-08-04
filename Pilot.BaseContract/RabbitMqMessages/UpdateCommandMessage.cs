namespace Pilot.Contracts.RabbitMqMessages;

public record UpdateCommandMessage<TDto>(TDto Value, int UserId) : BaseCommandMessage<TDto>(Value, UserId);
