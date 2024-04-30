using MassTransit;
using MediatR;
using Pilot.Contracts.RabbitMqMessages.Company;

namespace Pilot.Api.Handlers.CompanyHandlers;

public record AddCompanyCommand(string CompanyName, string UserId) : IRequest;

public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public AddCompanyCommandHandler(
        IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new TitleCompany(request.UserId, request.CompanyName), cancellationToken);
    }
}