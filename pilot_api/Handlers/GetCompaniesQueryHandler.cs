using MediatR;
using pilot_api.Models;
using pilot_api.Queries;
using pilot_api.Repository;

namespace pilot_api.Handlers;

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, ICollection<Company>>
{
    private readonly CompanyRepository _company;
    private readonly ILogger<GetCompaniesQueryHandler> _logger;

    public GetCompaniesQueryHandler(CompanyRepository company, ILogger<GetCompaniesQueryHandler> logger)
    {
        _company = company;
        _logger = logger;
    }
    
    public async Task<ICollection<Company>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get all companies");

        var list = await _company.GetCompaniesAsync(cancellationToken);

        _logger.LogInformation("Successfully getting companies");
        return list;
    }
}