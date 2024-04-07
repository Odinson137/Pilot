using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.DTO;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Queries;

public record GetCompanyUsersQuery(string CompanyId, string CacheKey) : IRequest<ICollection<CompanyUserDto>>, ICacheableMediatrQuery;
