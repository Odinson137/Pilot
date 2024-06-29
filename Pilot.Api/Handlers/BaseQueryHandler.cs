using MediatR;
using Pilot.Api.Queries;
using Pilot.Contracts.Base;

namespace Pilot.Api.Handlers;

public class BaseQueryHandler<T, TDto>(IBaseRepository<T> repository) :
    IRequestHandler<GetValuesQuery<TDto>, ICollection<TDto>>,
    IRequestHandler<GetValueByIdQuery<TDto>, TDto?> where TDto : BaseId
{
    public async Task<ICollection<TDto>> Handle(GetValuesQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await repository.GetAllValuesAsync<TDto>(request.Skip, request.Take, token: cancellationToken);
    }

    public async Task<TDto?> Handle(GetValueByIdQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync<TDto>(request.Id, cancellationToken);
    }
}