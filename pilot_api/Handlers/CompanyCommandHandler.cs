using MassTransit;
using MediatR;
using pilot_api.Commands;
using pilot_api.Models;
using pilot_api.Repository;
using Pilot.Contracts;

namespace pilot_api.Handlers;

public class CompanyCommandHandler : IRequestHandler<AddCompanyCommand, Company>
{
    private readonly CompanyRepository _company;
    private readonly ILogger<CompanyCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;

    public CompanyCommandHandler(CompanyRepository company,
        ILogger<CompanyCommandHandler> logger,
        IPublishEndpoint publishEndpoint, IBus bus)
    {
        _company = company;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _bus = bus;
    }

    public CompanyCommandHandler(CompanyRepository company)
    {
        _company = company;
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
