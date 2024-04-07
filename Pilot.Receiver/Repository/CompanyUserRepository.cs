using MongoDB.Driver;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Repository;

public class CompanyUserRepository : ICompanyUser
{
    private readonly IMongoCollection<Company> _mongoCollection;
    
    public CompanyUserRepository(IMongoDatabase mongoDatabase)
    {
        _mongoCollection = mongoDatabase.GetCollection<Company>(MongoTable.Company);
    }
    
    public async Task<ICollection<CompanyUserDto>> GetCompanyUsersAsync(string companyId)
    {
        var filter = Builders<Company>.Filter.Eq(c => c.Id, companyId);
        var companyUsers = await _mongoCollection
            .Find(filter)
            .Project(cu => cu.CompanyUsers
                .Select(c => new CompanyUserDto(c.Id, c.UserName, c.Name, c.LastName, c.Timestamp, c.Role)))
            .FirstOrDefaultAsync();

        if (companyUsers == null)
        {
            throw new NotFoundException("Company not found");
        }
        
        return companyUsers.ToList();
    }

    public async Task<CompanyUserDto> GetCompanyUserAsync(string companyId, string userId)
    {
        var filter = Builders<Company>.Filter.Eq(c => c.Id, companyId);
        var companyUsers = await _mongoCollection
            .Find(filter)
            .Project(cu => cu.CompanyUsers
                .Select(c => new CompanyUserDto(c.Id, c.UserName, c.Name, c.LastName, c.Timestamp, c.Role)))
            .FirstOrDefaultAsync();

        if (companyUsers == null)
        {
            throw new NotFoundException("Company not found");
        }
        
        var user = companyUsers.FirstOrDefault(c => c.Id == userId)!;

        if (user == null)
        {
            throw new NotFoundException("User in company not found");
        }

        return user;
    }
}
