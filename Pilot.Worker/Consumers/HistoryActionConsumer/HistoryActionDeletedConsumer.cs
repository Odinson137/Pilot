using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.HistoryActionConsumer;

public class HistoryActionDeletedConsumer(
    ILogger<HistoryActionDeletedConsumer> logger,
    IHistoryAction repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<HistoryAction, HistoryActionDto>(logger, repository, message, validate)
{
}