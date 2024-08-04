namespace Pilot.Contracts.RabbitMqMessages;

public record CreateCommandMessage<TDto>(TDto Value, int UserId) : BaseCommandMessage<TDto>(Value, UserId);