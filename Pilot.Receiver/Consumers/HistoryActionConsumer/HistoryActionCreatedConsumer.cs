using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Consumers.HistoryActionConsumer;

public class HistoryActionCreatedConsumer(
    ILogger<HistoryActionCreatedConsumer> logger,
    IHistoryAction repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<HistoryAction, HistoryActionDto>(logger, repository, messageService, validate, mapper, companyUser)
{
}