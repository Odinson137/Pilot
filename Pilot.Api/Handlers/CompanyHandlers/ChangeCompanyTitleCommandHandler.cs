using MassTransit;
using MediatR;
using Pilot.Contracts.RabbitMqMessages.Company;

namespace Pilot.Api.Handlers.CompanyHandlers;

public record ChangeCompanyTitleCommand(string CompanyId, string CompanyName, string UserId) : IRequest;

public class ChangeCompanyTitleCommandHandler : IRequestHandler<ChangeCompanyTitleCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;


    public ChangeCompanyTitleCommandHandler(
        IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(ChangeCompanyTitleCommand request, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(
            new ChangeTitleCompany(request.UserId, request.CompanyId, request.CompanyName), 
            cancellationToken);
    }
}