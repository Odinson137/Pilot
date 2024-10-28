using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Storage.Queries;

public record GetFileUrlsQuery<TDto> 
    : BaseQuery, IRequest<ICollection<TDto>> where TDto : BaseDto
{
    public GetFileUrlsQuery(BaseFilter? filter)
    {
        Filter = filter;
    }

    public BaseFilter? Filter { get; set; }
}