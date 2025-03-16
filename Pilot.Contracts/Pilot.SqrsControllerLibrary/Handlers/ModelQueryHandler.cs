using MediatR;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.SqrsControllerLibrary.Handlers;

public abstract class ModelQueryHandler<T, TDto> : 
    IRequestHandler<GetValuesQuery<TDto>, ICollection<TDto>>,
    IRequestHandler<GetValueByIdQuery<TDto>, TDto>
    where TDto : BaseDto where T : BaseModel
{
    protected readonly IBaseReadRepository<T> Repository;
    protected readonly ILogger<ModelQueryHandler<T, TDto>> Logger;
    
    public ModelQueryHandler(IBaseReadRepository<T> repository, ILogger<ModelQueryHandler<T, TDto>> logger)
    {
        Repository = repository;
        Logger = logger;
    }

    public async Task<TDto> Handle(GetValueByIdQuery<TDto> request, CancellationToken cancellationToken)
    {
        var result = await Repository.GetByIdAsync<TDto>(request.Id, cancellationToken);
        if (result == null) throw new NotFoundException($"{typeof(TDto).Namespace} not found");
        return result;
    }

    public async Task<ICollection<TDto>> Handle(GetValuesQuery<TDto> request, CancellationToken cancellationToken)
    {
        var result = await Repository.GetValuesAsync<TDto>(request.Filter ?? new BaseFilter(), cancellationToken);
        return result;
    }
}