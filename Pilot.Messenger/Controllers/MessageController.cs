using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Messenger.Controllers;

public class MessageController(IMediator mediator) : PilotReadOnlyController<MessageDto>(mediator);