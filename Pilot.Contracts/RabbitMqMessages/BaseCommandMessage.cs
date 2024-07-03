namespace Pilot.Contracts.RabbitMqMessages;

public record BaseCommandMessage<TDto>(TDto Value, string UserId)
{
    public TDto Value { get; set; } = Value;
    public string UserId { get; set; } = UserId;
}