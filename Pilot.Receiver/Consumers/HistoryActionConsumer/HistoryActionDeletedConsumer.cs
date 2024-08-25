using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Consumers.HistoryActionConsumer;

public class HistoryActionDeletedConsumer(
    ILogger<HistoryActionDeletedConsumer> logger,
    IHistoryAction repository,
    IMessageService message,
    IBaseValidatorService validate)
    : BaseDeleteConsumer<HistoryAction, HistoryActionDto>(logger, repository, message, validate)
{
}