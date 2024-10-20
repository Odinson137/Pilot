using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Messenger.Controllers;

public class InfoMessageController(IMediator mediator) : PilotReadOnlyController<InfoMessageDto>(mediator);