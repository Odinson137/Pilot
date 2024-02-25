using System.Net.Mime;
using MassTransit;
using MediatR;
using pilot_api.Commands;
using pilot_api.Models;
using pilot_api.Repository;
using RabbitMqMessages;

namespace pilot_api.Handlers;

public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, Company>
{
    private readonly CompanyRepository _company;
    private readonly ILogger<AddCompanyCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;
    
    public AddCompanyCommandHandler(CompanyRepository company, 
        ILogger<AddCompanyCommandHandler> logger, 
        IPublishEndpoint publishEndpoint, IBus bus)
    {
        _company = company;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _bus = bus;
    }

    public async Task<Company> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Add company");

        await _publishEndpoint.Publish<IMessage>(new { Text = request.CompanyName }, cancellationToken);

        // var endpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/message_queue"));
        // await endpoint.Send<IMessage>(new { Text = "hello" }, cancellationToken);

        _logger.LogInformation("The company has been added");
        return new Company() { Title = request.CompanyName };
    }
}

