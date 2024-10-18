using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class TaskInfoCommandHandler(IBaseMassTransitService massTransitService)
    : ModelCommandHandler<TaskInfoDto>(massTransitService);