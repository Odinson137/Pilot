using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsController.Controller;

namespace Pilot.Messenger.Controllers;

public class MessageController(IMediator mediator) : PilotController<MessageDto>(mediator);
