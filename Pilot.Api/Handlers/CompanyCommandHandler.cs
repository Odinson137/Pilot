using MassTransit;
using MediatR;

namespace Pilot.Api.Handlers;

public class CompanyCommandHandler<TDto>(
    ILogger<CompanyCommandHandler<TDto>> logger,
    IPublishEndpoint publishEndpoint)
    : BaseCommandHandlers<TDto>(logger, publishEndpoint)
    where TDto : IRequest;
