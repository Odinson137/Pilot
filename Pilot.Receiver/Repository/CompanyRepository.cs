using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Contracts.Models;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class CompanyRepository : ICompany
{
    private readonly IMongoCollection<Company> _companyCollection;

    public CompanyRepository(IMongoDatabase mongoDatabase)
    {
        _companyCollection = mongoDatabase.GetCollection<Company>(MongoTable.Company);
    }

    public async Task<Company?> CheckCompanyTitleExistAsync(string title)
    {
        var filter = Builders<Company>.Filter.Eq(c => c.Title, title);
        var company = await _companyCollection.Find(filter).FirstOrDefaultAsync();
        return company;
    }

    public async Task AddCompanyAsync(Company company)
    {
        await _companyCollection.InsertOneAsync(company);
    }

    public async Task ChangeCompanyTitleAsync(string companyId, string companyTitle)
    {
        var searchFilter = Builders<Company>.Filter.Eq(c => c.Id, companyId);
        var updateFilter = Builders<Company>.Update.Rename(c => c.Id, companyId);
        await _companyCollection.FindOneAndUpdateAsync(searchFilter, updateFilter);
    }
}