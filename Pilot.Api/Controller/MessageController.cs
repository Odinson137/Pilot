using MediatR;
using Pilot.Api.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;

namespace Pilot.Api.Controller;

public class MessageController(IMediator mediator) : GatewayController<Message, MessageDto>(mediator);
