using MediatR;
using Pilot.Contracts.Base;
using Pilot.InvalidationCacheRedisLibrary.CacheKeyTemplates;
using Pilot.SqrsControllerLibrary.Interfaces;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.SqrsControllerLibrary.Commands;

public record GetValueByIdQuery<TDto>(int Id)
    : BaseQuery, IRequest<TDto>, ICacheableOneMediatrQuery, IQueryOneHandling where TDto : BaseDto
{
    public BaseFilter Filter { get; set; }

    public string CacheKey => BaseCacheKeyTemplate.OneCacheKey(GetModelName<TDto>(), Id);
    public string Url => $"api/{GetModelName<TDto>()}/{Id}";
}

public record GetValuesQuery<TDto> 
    : BaseQuery, IRequest<ICollection<TDto>>, ICacheableListMediatrQuery, IQueryListHandling where TDto : BaseDto
{
    public GetValuesQuery(BaseFilter filter)
    {
        Filter = filter;
    }

    public BaseFilter Filter { get; set; }

    public string CacheKey => Filter.GetHashCode().ToString();

    public string Url => $"api/{GetModelName<TDto>()}";
}