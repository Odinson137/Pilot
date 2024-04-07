using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.DTO;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Queries;

public record GetCompanyByIdQuery(string Id, string CacheKey) : IRequest<CompanyDto>, ICacheableMediatrQuery;

