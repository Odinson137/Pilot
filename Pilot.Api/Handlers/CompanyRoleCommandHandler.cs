using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class CompanyRoleCommandHandler(IBaseMassTransitService massTransitService)
    : ModelCommandHandler<CompanyRoleDto>(massTransitService);