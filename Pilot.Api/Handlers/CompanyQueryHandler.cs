using MediatR;
using Pilot.Api.DTO;
using Pilot.Api.Interfaces.Repositories;
using Pilot.Api.Queries;
using Pilot.Api.Repository;

namespace Pilot.Api.Handlers;

public class CompanyQueryHandler : 
    IRequestHandler<GetCompaniesQuery, ICollection<CompanyDto>>,
    IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly ICompany _company;
    private readonly ILogger<CompanyQueryHandler> _logger;

    public CompanyQueryHandler(ICompany company, ILogger<CompanyQueryHandler> logger)
    {
        _company = company;
        _logger = logger;
    }

    public async Task<ICollection<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get all companies");

        var list = await _company.GetCompaniesAsync(cancellationToken);

        _logger.LogInformation("Successfully getting companies");
        return list;
    }

    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get company by id");

        var company = await _company.GetCompanyAsync(request.Id, cancellationToken);
        
        _logger.LogInformation("Successfully getting company by id");

        return company;
    }
}