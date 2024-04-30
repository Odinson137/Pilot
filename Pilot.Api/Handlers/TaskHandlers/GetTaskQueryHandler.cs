using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.Interfaces.Services;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Handlers.TaskHandlers;
public record GetTaskQuery(string CompanyId, string ProjectId, string TaskId, string CacheKey) 
    : IRequest<TaskDto>, ICacheableMediatrQuery;

public class GetTaskQueryHandler
    : IRequestHandler<GetTaskQuery, TaskDto>

{
    private readonly IQueryService _queryService;

    public GetTaskQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }
    
    public async Task<TaskDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.SendRequestAsync<TaskDto>($"Project/{request.CompanyId}/{request.ProjectId}", 
            cancellationToken);
    }
}