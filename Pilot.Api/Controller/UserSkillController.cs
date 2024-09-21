using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class UserSkillController(IMediator mediator) : PilotReadOnlyController<UserSkillDto>(mediator);