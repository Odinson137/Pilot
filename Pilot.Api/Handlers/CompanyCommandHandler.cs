using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class CompanyCommandHandler(IBaseMassTransitService massTransitService)
    : ModelCommandHandler<CompanyDto>(massTransitService);