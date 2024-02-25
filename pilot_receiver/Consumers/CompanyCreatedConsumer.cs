using MassTransit;
using RabbitMqMessages;

namespace pilot_receiver.Consumers;

public class CompanyCreatedConsumer : IConsumer<IMessage>
{
    private readonly ILogger<CompanyCreatedConsumer> _logger;
    
    public CompanyCreatedConsumer(ILogger<CompanyCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<IMessage> context)
    {
        _logger.LogInformation("Company create consume");
        _logger.LogInformation(context.Message.Text);
        return Task.CompletedTask;
    }
}