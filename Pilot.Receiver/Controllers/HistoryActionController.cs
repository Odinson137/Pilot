using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Controllers;

public class HistoryActionController(IHistoryAction repository, ILogger<HistoryActionController> logger) : BaseSelectController<HistoryAction, HistoryActionDto>(repository, logger);