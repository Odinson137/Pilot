using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.DTO;

namespace Pilot.Api.Queries;

public record GetCompaniesQuery(string CacheKey) : IRequest<ICollection<CompanyDto>>, ICacheableMediatrQuery;
