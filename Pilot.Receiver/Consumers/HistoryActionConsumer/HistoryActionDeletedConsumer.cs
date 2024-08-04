using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Consumers.HistoryActionConsumer;

public class HistoryActionDeletedConsumer(
    ILogger<HistoryActionDeletedConsumer> logger,
    IHistoryAction repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseDeleteConsumer<HistoryAction, HistoryActionDto>(logger, repository, message, validate, mapper)
{

}