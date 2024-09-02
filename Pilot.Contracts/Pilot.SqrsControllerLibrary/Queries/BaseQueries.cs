using MediatR;
using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Queries;

public record GetValueByIdQuery<TDto>(int Id)
    : BaseQuery, IRequest<TDto> where TDto : BaseDto;

public record GetValuesQuery<TDto> 
    : BaseQuery, IRequest<ICollection<TDto>> where TDto : BaseDto
{
    public GetValuesQuery(BaseFilter filter)
    {
        Filter = filter;
    }

    public BaseFilter Filter { get; set; }
}