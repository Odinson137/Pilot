using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.HistoryActionConsumer;

public class HistoryActionCreatedConsumer(
    ILogger<HistoryActionCreatedConsumer> logger,
    IHistoryAction repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<HistoryAction, HistoryActionDto>(logger, repository, messageService, validate, mapper,
        companyUser)
{
}