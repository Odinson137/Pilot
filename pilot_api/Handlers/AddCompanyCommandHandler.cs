using MediatR;
using pilot_api.Commands;
using pilot_api.Models;
using pilot_api.Repository;

namespace pilot_api.Handlers;

public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, Company>
{
    private readonly CompanyRepository _company;

    public AddCompanyCommandHandler(CompanyRepository company)
    {
        _company = company;
    }

    public async Task<Company> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = new Company
        {
            CompanyId = Guid.NewGuid().ToString(),
            Title = request.companyName,
        };

        await _company.AddCompanyAsync(company);

        return company;
    }
}