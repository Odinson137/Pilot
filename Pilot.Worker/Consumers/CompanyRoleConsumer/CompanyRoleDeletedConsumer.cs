using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.CompanyRoleConsumer;

public class CompanyRoleDeletedConsumer(
    ILogger<CompanyRoleDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<CompanyRole, CompanyRoleDto>(logger, mediator)
{
}