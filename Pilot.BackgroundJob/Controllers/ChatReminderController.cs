using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.BackgroundJob.Controllers;

public class ChatReminderController(IMediator mediator) : PilotReadOnlyController<ChatReminderDto>(mediator);