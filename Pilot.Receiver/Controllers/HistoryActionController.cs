using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Controllers;

public class HistoryActionController(IHistoryAction repository, ILogger<HistoryActionController> logger) : BaseSelectController<HistoryAction, HistoryActionDto>(repository, logger);