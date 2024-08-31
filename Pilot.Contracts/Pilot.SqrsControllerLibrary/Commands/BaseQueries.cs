using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.SqrsControllerLibrary.Commands;

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