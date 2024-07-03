using MediatR;
using Pilot.Api.Data.ControllerSettings;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Api.Controller;

public class FileController(IMediator mediator) : PilotController<File, FileDto>(mediator);
