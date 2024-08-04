using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsController.Interfaces;
using Pilot.SqrsController.Queries;

namespace Pilot.SqrsController.Commands;

public record GetValueByIdQuery<TDto>(int Id) : BaseQuery, IRequest<TDto>, ICacheableMediatrQuery, IQueryOneHandling where TDto : BaseDto
{
    public string Url => $"api/{GetModelName<TDto>()}/{Id}";
    
    public BaseFilter? Filter { get; set; }

    public string CacheKey => $"{GetModelName<TDto>()}-{Id}";
}

public record GetValuesQuery<TDto> : BaseQuery, IRequest<ICollection<TDto>>, ICacheableMediatrQuery, IQueryListHandling where TDto : BaseDto
{
    public BaseFilter? Filter { get; set; }

    public GetValuesQuery(int skip, int take)
    {
        Filter = new BaseFilter
        {
            Skip = skip,
            Take = take
        };
    }
    
    public string CacheKey => $"all-{GetModelName<TDto>()}-{Filter!.Skip}-{Filter.Take}";
    
    public string Url => $"api/{GetModelName<TDto>()}";
}