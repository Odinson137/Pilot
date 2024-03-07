using MassTransit;
using MediatR;
using Pilot.Api.Commands;
using Pilot.Api.Models;
using Pilot.Api.Repository;
using Pilot.Contracts;

namespace Pilot.Api.Handlers;

public class CompanyCommandHandler : IRequestHandler<AddCompanyCommand, Company>
{
    private readonly CompanyRepository _company;
    private readonly ILogger<CompanyCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public CompanyCommandHandler(CompanyRepository company,
        ILogger<CompanyCommandHandler> logger,
        IPublishEndpoint publishEndpoint)
    {
        _company = company;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
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
