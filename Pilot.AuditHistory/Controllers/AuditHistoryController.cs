using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.AuditHistory.Controllers;

public class AuditHistoryController(IMediator mediator) : PilotReadOnlyController<AuditHistoryDto>(mediator);
