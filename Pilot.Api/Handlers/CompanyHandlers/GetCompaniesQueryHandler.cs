using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.Interfaces.Services;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Handlers.CompanyHandlers;

public record GetCompaniesQuery(string CacheKey) : IRequest<ICollection<CompanyDto>>, ICacheableMediatrQuery;

public class GetCompaniesQueryHandler : 
    IRequestHandler<GetCompaniesQuery, ICollection<CompanyDto>>
{
    private readonly IQueryService _queryService;

    public GetCompaniesQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }
    
    public async Task<ICollection<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.SendRequestAsync<ICollection<CompanyDto>>("/Company", cancellationToken);
    }
}
