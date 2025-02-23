using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyUserConsumer;

public class CompanyUserUpdatedConsumer(
    ILogger<CompanyUserUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<CompanyUser, CompanyUserDto>(logger, mediator)
{
}