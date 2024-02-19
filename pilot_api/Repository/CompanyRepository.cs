using System.Globalization;
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
                CompanyId = Guid.NewGuid().ToString(),
                Title = "Apple",
            },
            new()
            {
                CompanyId = Guid.NewGuid().ToString(),
                Title = "Microsoft",
            },
            new()
            {
                CompanyId = Guid.NewGuid().ToString(),
                Title = "Amazon",
            },
            new ()
            {
                CompanyId = Guid.NewGuid().ToString(),
                Title = "ITechArt"
            },
            new ()
            {
                CompanyId = Guid.NewGuid().ToString(),
                Title = "Intel"
            }
        };
    }

    public async Task<ICollection<Company>> GetCompaniesAsync()
    {
        return await Task.FromResult(companies);
    }
    
    public async Task<Company> GetCompany(string id)
    {
        return await Task.FromResult(companies.First(c => c.CompanyId == id));
    }

    public Task AddCompanyAsync(Company company)
    {
        companies.Add(company);
        return Task.CompletedTask;
    }
}