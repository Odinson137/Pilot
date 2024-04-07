using MongoDB.Bson;
using MongoDB.Driver;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Repository;

public class CompanyRepository : ICompany
{
    private readonly IMongoCollection<Company> _companyCollection;

    public CompanyRepository(IMongoDatabase mongoDatabase)
    {
        _companyCollection = mongoDatabase.GetCollection<Company>(MongoTable.Company);
    }

    public async Task<ICollection<CompanyDto>> GetCompaniesAsync(CancellationToken cancellationToken)
    {
        var companies = await _companyCollection.Find(new BsonDocument())
            .Project(u => new CompanyDto(u.Id, u.Title, u.Description))
            .ToListAsync(cancellationToken);
        
        return companies;
    }
    
    public async Task<CompanyDto> GetCompanyAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Company>.Filter.Eq(u => u.Id, id);
        
        var company = await _companyCollection.Find(filter)
            .Project(u => new CompanyDto(u.Id, u.Title, u.Description))
            .FirstOrDefaultAsync(cancellationToken);
        
        return company;
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