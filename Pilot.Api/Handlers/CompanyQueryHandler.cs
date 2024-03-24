using MediatR;
using Pilot.Api.DTO;
using Pilot.Api.Interfaces.Repositories;
using Pilot.Api.Queries;

namespace Pilot.Api.Handlers;

public class CompanyQueryHandler : 
    IRequestHandler<GetCompaniesQuery, ICollection<CompanyDto>>,
    IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly ICompany _company;

    public CompanyQueryHandler(ICompany company)
    {
        _company = company;
    }

    public async Task<ICollection<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var list = await _company.GetCompaniesAsync(cancellationToken);
        return list;
    }

    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _company.GetCompanyAsync(request.Id, cancellationToken);
        return company;
    }
}