using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Api.Handlers.BaseHandlers;

public class ModelQueryHandler<TDto> : 
    IRequestHandler<GetValueByIdQuery<TDto>, TDto?>,
    IRequestHandler<GetValuesQuery<TDto>, ICollection<TDto>>,
    IRequestHandler<GetQueryValueQuery<TDto>, string>
    where TDto : BaseDto
{
    private readonly IModelService _modelService;

    public ModelQueryHandler(IModelService modelService)
    {
        _modelService = modelService;
    }
    
    public async Task<TDto?> Handle(GetValueByIdQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await _modelService.GetValueByIdAsync<TDto>(request.Id, cancellationToken);
    }

    public async Task<ICollection<TDto>> Handle(GetValuesQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await _modelService.GetValuesAsync<TDto>(request.Url ?? "", request.Filter, cancellationToken);
    }

    public async Task<string> Handle(GetQueryValueQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await _modelService.GetQueryValueAsync<TDto>(request.Filter, cancellationToken);
    }
}