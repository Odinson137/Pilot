using System.Globalization;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Pilot.Api.DTO;
using Pilot.Api.Models;

namespace Pilot.Api.Repository;

public class CompanyRepository
{
    private readonly IMongoDatabase _mongoDatabase;
    public CompanyRepository(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public async Task<ICollection<CompanyDto>> GetCompaniesAsync(CancellationToken cancellationToken)
    {
        var companiesCollection = _mongoDatabase.GetCollection<Company>("companies");
        
        var companies = await companiesCollection.Find(new BsonDocument())
            .Project(u => new CompanyDto(u.Id, u.Title, u.Description))
            .ToListAsync(cancellationToken);
        
        return companies;
    }
    
    public async Task<CompanyDto> GetCompanyAsync(string id, CancellationToken cancellationToken)
    {
        var companiesCollection = _mongoDatabase.GetCollection<Company>("companies");
        
        var filter = Builders<Company>.Filter.Eq(u => u.Id, id);
        
        var company = await companiesCollection.Find(filter)
            .Project(u => new CompanyDto(u.Id, u.Title, u.Description))
            .FirstOrDefaultAsync(cancellationToken);
        
        return company;
    }
}