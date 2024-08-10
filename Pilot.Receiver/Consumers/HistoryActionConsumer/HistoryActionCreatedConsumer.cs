using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.HistoryActionConsumer;

public class HistoryActionCreatedConsumer(
    ILogger<HistoryActionCreatedConsumer> logger,
    IHistoryAction repository,
    IMessageService messageService,
    IBaseValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<HistoryAction, HistoryActionDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}