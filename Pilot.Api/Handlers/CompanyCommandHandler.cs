using MassTransit;
using MediatR;
using Pilot.Api.Commands;
using Pilot.Api.Repository;
using Pilot.Contracts;
using Pilot.Contracts.RabbitMqMessages.Company;

namespace Pilot.Api.Handlers;

public class CompanyCommandHandler : IRequestHandler<CompanyAddCommand>
{
    private readonly ILogger<CompanyCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;


    public CompanyCommandHandler(
        ILogger<CompanyCommandHandler> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(CompanyAddCommand request, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new TitleCompany(request.UserId, request.CompanyName), cancellationToken);
    }
}
