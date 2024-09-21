using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class HistoryHandler : ModelQueryHandler<HistoryAction, HistoryActionDto>
{
    public HistoryHandler(IHistoryAction repository, ILogger<ModelQueryHandler<HistoryAction, HistoryActionDto>> logger) : base(repository, logger)
    {
    }
}