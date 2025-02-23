using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TeamEmployeeConsumer;

public class TeamEmployeeDeletedConsumer(
    ILogger<TeamEmployeeDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<TeamEmployee, TeamEmployeeDto>(logger, mediator);