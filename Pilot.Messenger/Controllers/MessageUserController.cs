using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Messenger.Controllers;

public class MessageUserController(IMediator mediator) : PilotReadOnlyController<MessageDto>(mediator);