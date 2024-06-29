using MediatR;
using Pilot.Api.Behaviors;

namespace Pilot.Api.Queries;

public record GetValueByIdQuery<TDto>(int Id, string CacheKey) : IRequest<TDto?>, ICacheableMediatrQuery;

public record GetValuesQuery<TDto>(int Skip, int Take, string CacheKey) : IRequest<ICollection<TDto>>, ICacheableMediatrQuery;