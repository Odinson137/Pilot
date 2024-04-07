using Pilot.Api.Data.Enums;

namespace Pilot.Contracts.RabbitMqMessages.Message;

public class Message
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string UserId { get; set; }
    public required MessagePriority MessagePriority { get; set; }
}