using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.HistoryActionConsumer;

public class HistoryActionDeletedConsumer(
    ILogger<HistoryActionDeletedConsumer> logger,
    IMediator mediator)
    : BaseDeleteConsumer<HistoryAction, HistoryActionDto>(logger, mediator)
{
}