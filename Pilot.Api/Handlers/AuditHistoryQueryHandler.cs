using MediatR;
using Pilot.Api.Handlers.BaseHandlers;
using Pilot.Api.Queries;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Api.Handlers;

public class AuditHistoryQueryHandler(IModelService modelService) : ModelQueryHandler<AuditHistoryDto>(modelService);
