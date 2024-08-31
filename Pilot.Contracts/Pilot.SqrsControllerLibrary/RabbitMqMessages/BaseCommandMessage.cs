namespace Pilot.SqrsControllerLibrary.RabbitMqMessages;

public record BaseCommandMessage<TDto>(TDto Value, int UserId)
{
    public TDto Value { get; set; } = Value;
    public int UserId { get; set; } = UserId;
}