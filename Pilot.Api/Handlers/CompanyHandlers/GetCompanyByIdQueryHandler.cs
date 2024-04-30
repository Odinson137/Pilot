using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.Interfaces.Services;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Handlers.CompanyHandlers;
public record GetCompanyByIdQuery(string Id, string CacheKey) : IRequest<CompanyDto>, ICacheableMediatrQuery;

public class GetCompanyByIdQueryHandler : 
    IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly IQueryService _queryService;

    public GetCompanyByIdQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }
    
    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.SendRequestAsync<CompanyDto>($"/Company/{request.Id}", cancellationToken);
    }
}
