using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.Interfaces.Services;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Handlers.ProjectHandlers;

public record GetProjectsQuery(string CompanyId, string CacheKey) : IRequest<ICollection<ProjectDto>>, ICacheableMediatrQuery;

public class GetProjectsQueryHandler 
    : IRequestHandler<GetProjectsQuery, ICollection<ProjectDto>>
{
    private readonly IQueryService _queryService;

    public GetProjectsQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }
    
    public async Task<ICollection<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.SendRequestAsync<ICollection<ProjectDto>>($"Project/{request.CompanyId}", cancellationToken);
    }
}