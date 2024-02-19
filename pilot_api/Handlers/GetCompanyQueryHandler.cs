using MediatR;
using pilot_api.Models;
using pilot_api.Queries;
using pilot_api.Repository;

namespace pilot_api.Handlers;

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyByIdQuery, Company>
{
    private readonly CompanyRepository _company;

    public GetCompanyQueryHandler(CompanyRepository company)
    {
        _company = company;
    }

    public Task<Company> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        return _company.GetCompany(request.id);
    }
}