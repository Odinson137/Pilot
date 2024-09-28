using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Storage.Controllers;

public class FileController(IMediator mediator) : PilotReadOnlyController<FileDto>(mediator)
{

}