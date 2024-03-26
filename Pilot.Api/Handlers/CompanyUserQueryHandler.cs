using MediatR;
using Pilot.Api.DTO;
using Pilot.Api.Interfaces.Repositories;
using Pilot.Api.Queries;

namespace Pilot.Api.Handlers;

public class CompanyUserQueryHandler :
    IRequestHandler<GetCompanyUsersQuery, ICollection<CompanyUserDto>>
{
    private readonly ICompanyUser _companyUser;
    
    public CompanyUserQueryHandler(ICompanyUser companyUser)
    {
        _companyUser = companyUser;
    }
    
    public async Task<ICollection<CompanyUserDto>> Handle(GetCompanyUsersQuery request, CancellationToken cancellationToken)
    {
        return await _companyUser.GetUserCompanyAsync(request.CompanyId);
    }
}