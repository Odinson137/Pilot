using MediatR;
using Pilot.Api.Behaviors;
using Pilot.Api.Interfaces.Services;
using Pilot.Contracts.DTO;

namespace Pilot.Api.Handlers.CompanyUserHandlers;

public record GetCompanyUsersQuery(string CompanyId, string CacheKey) : IRequest<ICollection<CompanyUserDto>>, ICacheableMediatrQuery;

public class GetCompanyUsersQueryHandler : 
    IRequestHandler<GetCompanyUsersQuery, ICollection<CompanyUserDto>>
{
    private readonly IQueryService _queryService;

    public GetCompanyUsersQueryHandler(IQueryService queryService)
    {
        _queryService = queryService;
    }
    
    public async Task<ICollection<CompanyUserDto>> Handle(GetCompanyUsersQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.SendRequestAsync<ICollection<CompanyUserDto>>($"/CompanyUser/{request.CompanyId}", cancellationToken);
    }
}