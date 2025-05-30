﻿using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class TaskInfoCommandHandler(ITaskInfo repository, IMapper mapper, IBaseValidatorService validateService)
    : ModelCommandHandler<TaskInfo, TaskInfoDto>(repository, mapper, validateService);