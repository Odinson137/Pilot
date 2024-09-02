using MediatR;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.SqrsControllerLibrary.Handlers;

public abstract class ModelQueryHandler<T, TDto> : 
    IRequestHandler<GetValuesQuery<TDto>, ICollection<TDto>>,
    IRequestHandler<GetValueByIdQuery<TDto>, TDto>
    where TDto : BaseDto where T : BaseModel
{
    private readonly IBaseReadRepository<T> _repository;
    private readonly ILogger<ModelQueryHandler<T, TDto>> _logger;
    
    public ModelQueryHandler(IBaseReadRepository<T> repository, ILogger<ModelQueryHandler<T, TDto>> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<TDto> Handle(GetValueByIdQuery<TDto> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync<TDto>(request.Id, cancellationToken);
        if (result == null) throw new NotFoundException($"{typeof(TDto).Namespace} not found");
        _logger.LogClassInfo(result);
        return result;
    }

    public async Task<ICollection<TDto>> Handle(GetValuesQuery<TDto> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetValuesAsync<TDto>(request.Filter, cancellationToken);
        _logger.LogClassInfo(result);
        return result;
    }
}