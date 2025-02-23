using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyRoleConsumer;

public class CompanyRoleUpdatedConsumer(
    ILogger<CompanyRoleUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<CompanyRole, CompanyRoleDto>(logger, mediator)
{
}