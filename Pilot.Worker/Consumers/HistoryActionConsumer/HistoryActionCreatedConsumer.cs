using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Models;

namespace Pilot.Worker.Consumers.HistoryActionConsumer;

public class HistoryActionCreatedConsumer(
    ILogger<HistoryActionCreatedConsumer> logger,
    IMediator mediator)
    : BaseCreatedConsumer<HistoryAction, HistoryActionDto>(logger, mediator)
{
}