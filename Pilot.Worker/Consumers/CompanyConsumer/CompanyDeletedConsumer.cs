using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyDeletedConsumer(
    ILogger<CompanyDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<Models.Company, CompanyDto>(logger, mediator)
{
}