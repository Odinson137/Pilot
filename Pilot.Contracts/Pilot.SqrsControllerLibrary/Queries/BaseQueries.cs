using MediatR;
using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Queries;

public record GetValueByIdQuery<TDto>(int Id)
    : BaseQuery, IRequest<TDto> where TDto : BaseDto
{
    public string? Url { get; set; }
}

public record GetValuesQuery<TDto> 
    : BaseQuery, IRequest<ICollection<TDto>> where TDto : BaseDto
{
    public GetValuesQuery(BaseFilter filter)
    {
        Filter = filter;
    }

    public GetValuesQuery(BaseFilter filter, string url)
    {
        Filter = filter;
        Url = url;
    }
    
    public string? Url { get; set; }
    
    public BaseFilter Filter { get; set; }
}