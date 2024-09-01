using MediatR;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.SqrsControllerLibrary.Handlers;

public class GetValueQueryHandler<T, TDto> : IRequestHandler<GetValueByIdQuery<TDto>, TDto> where TDto : BaseDto where T : BaseModel
{
    private readonly IBaseReadRepository<T> _repository;
    private readonly ILogger<GetValueQueryHandler<T, TDto>> _logger;
    
    public GetValueQueryHandler(IBaseReadRepository<T> repository, ILogger<GetValueQueryHandler<T, TDto>> logger)
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
}