using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyUpdatedConsumer(
    ILogger<CompanyUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<Models.Company, CompanyDto>(logger, mediator)
{
}