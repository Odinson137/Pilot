using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Receiver.Handlers;

public class HistoryHandler : ModelQueryHandler<HistoryAction, HistoryActionDto>
{
    public HistoryHandler(IHistoryAction repository, ILogger<ModelQueryHandler<HistoryAction, HistoryActionDto>> logger) : base(repository, logger)
    {
    }
}