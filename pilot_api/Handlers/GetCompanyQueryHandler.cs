using MediatR;
using pilot_api.Models;
using pilot_api.Queries;
using pilot_api.Repository;

namespace pilot_api.Handlers;

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyByIdQuery, Company>
{
    private readonly CompanyRepository _company;
    private readonly ILogger<GetCompanyQueryHandler> _logger;

    public GetCompanyQueryHandler(CompanyRepository company, ILogger<GetCompanyQueryHandler> logger)
    {
        _company = company;
        _logger = logger;
    }

    public Task<Company> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get company");

        var company = _company.GetCompany(request.Id, cancellationToken);
        
        _logger.LogInformation("Successfully getting company");

        return company;
    }
}