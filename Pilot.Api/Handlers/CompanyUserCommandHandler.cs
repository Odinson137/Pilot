using MassTransit;
using MediatR;
using Pilot.Api.Commands;
using Pilot.Contracts.RabbitMqMessages.CompanyUser;

namespace Pilot.Api.Handlers;

public class CompanyUserCommandHandler : 
    IRequestHandler<AddCompanyUserCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CompanyUserCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Handle(AddCompanyUserCommand request, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(
            new CompanyUserAdded(request.UserId, request.AuthorId, request.CompanyId), cancellationToken);
    }
}