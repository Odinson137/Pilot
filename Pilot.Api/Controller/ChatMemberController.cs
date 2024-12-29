using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class ChatMemberController(IMediator mediator) : PilotController<ChatMemberDto>(mediator);