using MediatR;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.SqrsControllerLibrary.Handlers;

public class GetValuesQueryHandler<T, TDto> : IRequestHandler<GetValuesQuery<TDto>, ICollection<TDto>> where TDto : BaseDto where T : BaseModel
{
    private readonly IBaseReadRepository<T> _repository;
    private readonly ILogger<GetValuesQueryHandler<T, TDto>> _logger;
    
    public GetValuesQueryHandler(IBaseReadRepository<T> repository, ILogger<GetValuesQueryHandler<T, TDto>> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ICollection<TDto>> Handle(GetValuesQuery<TDto> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetValuesAsync<TDto>(request.Filter, cancellationToken);
        _logger.LogClassInfo(result);
        return result;
    }
}