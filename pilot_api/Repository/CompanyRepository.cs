using System.Globalization;
using MongoDB.Bson;
using pilot_api.Models;

namespace pilot_api.Repository;

public class CompanyRepository
{
    private ICollection<Company> companies { get; set; }

    public CompanyRepository()
    {
        companies = new List<Company>()
        {
            new()
            {
                Title = "Apple",
            },
            new()
            {
                Title = "Microsoft",
            },
            new()
            {
                Title = "Amazon",
            },
            new ()
            {
                Title = "ITechArt"
            },
            new ()
            {
                Title = "Intel"
            }
        };
    }

    public async Task<ICollection<Company>> GetCompaniesAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(companies);
    }
    
    public async Task<Company> GetCompany(string id, CancellationToken cancellationToken)
    {
        return await Task.FromResult(companies.First(c => c.Id.ToString() == id));
    }

    public async Task<Company> GetCompany(ObjectId id)
    {
        return await Task.FromResult(companies.First(c => c.Id == id));
    }

    public Task AddCompanyAsync(Company company)
    {
        companies.Add(company);
        return Task.CompletedTask;
    }
}