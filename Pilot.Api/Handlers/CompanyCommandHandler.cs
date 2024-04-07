using MassTransit;
using MediatR;
using Pilot.Api.Commands;
using Pilot.Contracts.RabbitMqMessages.Company;

namespace Pilot.Api.Handlers;

public class CompanyCommandHandler : IRequestHandler<AddCompanyCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CompanyCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new TitleCompany(request.UserId, request.CompanyName), cancellationToken);
    }
}
