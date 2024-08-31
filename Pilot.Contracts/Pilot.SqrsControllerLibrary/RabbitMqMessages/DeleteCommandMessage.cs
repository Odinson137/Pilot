namespace Pilot.SqrsControllerLibrary.RabbitMqMessages;

// ReSharper disable once UnusedTypeParameter
public record DeleteCommandMessage<TDto>(int Value, int UserId) : BaseCommandMessage<int>(Value, UserId);