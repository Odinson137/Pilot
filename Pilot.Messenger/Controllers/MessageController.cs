using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Pilot.SqrsController.Controller;

namespace Pilot.Messenger.Controllers;

public class MessageController(IMediator mediator) : PilotController<Message, MessageDto>(mediator);
