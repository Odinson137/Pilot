using MediatR;
using Pilot.Api.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Api.Controller;

public class FileController(IMediator mediator) : GatewayController<File, FileDto>(mediator);
