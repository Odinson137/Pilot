﻿using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class ProjectCommandHandler(IProject repository, IMapper mapper, IBaseValidatorService validateService)
    : ModelCommandHandler<Project, ProjectDto>(repository, mapper, validateService);