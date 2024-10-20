using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Consumers.TeamConsumer;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.TaskInfoConsumer;

public class TaskInfoCreatedConsumer(
    ILogger<TaskInfoCreatedConsumer> logger,
    ITaskInfo repository,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<TaskInfo, TaskInfoDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}