using MediatR;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;

namespace Pilot.Api.Controller;

public class MessageController(IMediator mediator) : PilotController<Message, MessageDto>(mediator);
