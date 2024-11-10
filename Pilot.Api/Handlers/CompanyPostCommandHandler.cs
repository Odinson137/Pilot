using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class CompanyPostCommandHandler(IBaseMassTransitService massTransitService)
    : ModelCommandHandler<CompanyPostDto>(massTransitService);