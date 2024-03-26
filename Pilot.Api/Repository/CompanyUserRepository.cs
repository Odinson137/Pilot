using MongoDB.Driver;
using Pilot.Api.Data;
using Pilot.Api.DTO;
using Pilot.Api.Interfaces.Repositories;
using Pilot.Contracts.Data;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Models;

namespace Pilot.Api.Repository;

public class CompanyUserRepository : ICompanyUser
{
    private readonly IMongoCollection<Company> _mongoCollection;
    
    public CompanyUserRepository(IMongoDatabase mongoDatabase)
    {
        _mongoCollection = mongoDatabase.GetCollection<Company>(MongoTable.Company);
    }
    
    public async Task<ICollection<CompanyUserDto>> GetUserCompanyAsync(string companyId)
    {
        var filter = Builders<Company>.Filter.Eq(c => c.Id, companyId);
        var companyUsers = await _mongoCollection
            .Find(filter)
            .Project(cu => cu.CompanyUsers
                .Select(c => new CompanyUserDto(c.Id, c.UserName, c.Name, c.LastName, c.Timestamp)))
            .FirstOrDefaultAsync();

        if (companyUsers == null)
        {
            throw new NotFoundException("Company not found");
        }
        
        return companyUsers.ToList();
    }
}
