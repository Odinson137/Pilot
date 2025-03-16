using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyCreatedConsumer(
    ILogger<CompanyCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<Company, CompanyDto>(logger, mediator)
{
}