using MassTransit;
using MediatR;
using Pilot.Contracts.RabbitMqMessages.CompanyUser;

namespace Pilot.Api.Handlers.CompanyUserHandlers;

public record AddCompanyUserCommand(string UserId, string AuthorId, string CompanyId) : IRequest;

public class AddCompanyUserCommandHandler : IRequestHandler<AddCompanyUserCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public AddCompanyUserCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Handle(AddCompanyUserCommand request, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(
            new CompanyUserAdded(request.UserId, request.AuthorId, request.CompanyId), cancellationToken);
    }
}