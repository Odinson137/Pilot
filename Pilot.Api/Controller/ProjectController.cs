﻿using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Controller;

namespace Pilot.Api.Controller;

public class ProjectController(IMediator mediator) : PilotController<ProjectDto>(mediator);