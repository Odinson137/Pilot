using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyRoleConsumer;

public class CompanyRoleCreatedConsumer(
    ILogger<CompanyRoleCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<CompanyRole, CompanyRoleDto>(logger, mediator)
{
}