using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Handlers;

public class HistoryQueryHandler : ModelQueryHandler<HistoryAction, HistoryActionDto>
{
    public HistoryQueryHandler(IHistoryAction repository, ILogger<ModelQueryHandler<HistoryAction, HistoryActionDto>> logger) : base(repository, logger)
    {
    }
}