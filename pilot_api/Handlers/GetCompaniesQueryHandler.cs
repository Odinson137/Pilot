using MediatR;
using pilot_api.Models;
using pilot_api.Queries;
using pilot_api.Repository;

namespace pilot_api.Handlers;

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, ICollection<Company>>
{
    private readonly CompanyRepository _company;

    public GetCompaniesQueryHandler(CompanyRepository company)
    {
        _company = company;
    }
    
    public async Task<ICollection<Company>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        return await _company.GetCompaniesAsync();
    }
}