using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.Api.Handlers;

public class GetValuesQueryHandler<TDto> : IRequestHandler<GetValuesQuery<TDto>, ICollection<TDto>> where TDto : BaseDto
{
    private readonly IModelService _modelService;
    
    public GetValuesQueryHandler(IModelService modelService)
    {
        _modelService = modelService;
    }

    public async Task<ICollection<TDto>> Handle(GetValuesQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await _modelService.GetValuesAsync<TDto>(request.Filter, cancellationToken);
    }
}