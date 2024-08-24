using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.Api.Handlers;

public class GetValueQueryHandler<TDto> : IRequestHandler<GetValueByIdQuery<TDto>, TDto> where TDto : BaseDto
{
    private readonly IModelService _modelService;
    
    public GetValueQueryHandler(IModelService modelService)
    {
        _modelService = modelService;
    }

    public async Task<TDto> Handle(GetValueByIdQuery<TDto> request, CancellationToken cancellationToken)
    {
        return await _modelService.GetValueByIdAsync<TDto>(request.Id, cancellationToken);
    }
}