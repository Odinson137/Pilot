using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Api.Handlers.BaseHandlers;

public class ModelQueryHandler<TDto> : 
    IRequestHandler<GetValueByIdQuery<TDto>, TDto>,
    IRequestHandler<GetValuesQuery<TDto>, ICollection<TDto>>
    where TDto : BaseDto
{
    private readonly IModelService _modelService;

    public ModelQueryHandler(IModelService modelService)
    {
        _modelService = modelService;
    }
    
    public async Task<TDto> Handle(GetValueByIdQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await _modelService.GetValueByIdAsync<TDto>(request.Id, cancellationToken);
    }

    public async Task<ICollection<TDto>> Handle(GetValuesQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await _modelService.GetValuesAsync<TDto>(request.Url ?? "", request.Filter, cancellationToken);
    }
}