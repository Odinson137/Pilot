namespace Pilot.Contracts.RabbitMqMessages;

public class BaseCommandMessage<TDto>
{
    public BaseCommandMessage(TDto value, string userId)
    {
        Value = value;
        UserId = userId;
    }

    public TDto Value { get; set; }
    public string UserId { get; set; }
}