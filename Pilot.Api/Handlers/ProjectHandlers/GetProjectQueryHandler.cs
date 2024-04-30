using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.Interfaces.Services;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Handlers.ProjectHandlers;
public record GetProjectQuery(string CompanyId, string ProjectId, string CacheKey) 
    : IRequest<ProjectDto>, ICacheableMediatrQuery;

public class GetCompanyProjectQueryHandler
    : IRequestHandler<GetProjectQuery, ProjectDto>

{
    private readonly IQueryService _queryService;

    public GetCompanyProjectQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }
    
    public async Task<ProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.SendRequestAsync<ProjectDto>($"Project/{request.CompanyId}/{request.ProjectId}", 
            cancellationToken);
    }
}