using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.DTO;

namespace Pilot.Api.Queries;

public record GetCompanyUsersQuery(string CompanyId, string CacheKey) : IRequest<ICollection<CompanyUserDto>>, ICacheableMediatrQuery;
