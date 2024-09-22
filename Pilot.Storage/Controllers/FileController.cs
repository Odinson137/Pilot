using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Storage.Controllers;

public class FileController(IMediator mediator) : PilotReadOnlyController<FileDto>(mediator)
{

}