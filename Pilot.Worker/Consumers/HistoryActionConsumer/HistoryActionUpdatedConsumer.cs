using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.HistoryActionConsumer;

public class HistoryActionUpdatedConsumer(
    ILogger<HistoryActionUpdatedConsumer> logger,
    IMediator mediator)
    : BaseUpdateConsumer<HistoryAction, HistoryActionDto>(logger, mediator)
{
}