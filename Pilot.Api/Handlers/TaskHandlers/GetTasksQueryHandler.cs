using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.Interfaces.Services;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Handlers.TaskHandlers;

public record GetTasksQuery(string CompanyId, string ProjectId, string CacheKey) : IRequest<ICollection<TaskDto>>, ICacheableMediatrQuery;

public class GetTasksQueryHandler 
    : IRequestHandler<GetTasksQuery, ICollection<TaskDto>>
{
    private readonly IQueryService _queryService;

    public GetTasksQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }
    
    public async Task<ICollection<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.SendRequestAsync<ICollection<TaskDto>>(
                $"Tasks/{request.CompanyId}/{request.ProjectId}", 
                cancellationToken);
    }
}