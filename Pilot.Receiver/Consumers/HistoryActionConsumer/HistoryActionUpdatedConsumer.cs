using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Models;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.HistoryActionConsumer;

public class HistoryActionUpdatedConsumer(
    ILogger<HistoryActionUpdatedConsumer> logger,
    IHistoryAction repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<HistoryAction, HistoryActionDto>(logger, repository, message, validate, mapper)
{
}