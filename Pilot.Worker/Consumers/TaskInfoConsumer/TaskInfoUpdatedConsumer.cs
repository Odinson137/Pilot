﻿using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TaskInfoConsumer;

public class TaskInfoUpdatedConsumer(
    ILogger<TaskInfoUpdatedConsumer> logger,
    ITaskInfo repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<TaskInfo, TaskInfoDto>(logger, repository, message, validate, mapper)
{
}