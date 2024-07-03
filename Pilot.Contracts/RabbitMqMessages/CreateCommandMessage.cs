namespace Pilot.Contracts.RabbitMqMessages;

public record CreateCommandMessage<TDto>(TDto Value, string UserId) : BaseCommandMessage<TDto>(Value, UserId);