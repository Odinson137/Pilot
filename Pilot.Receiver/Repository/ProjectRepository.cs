using MongoDB.Driver;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Receiver.Interface;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Repository;

public class ProjectRepository : IProject
{
    private readonly IMongoCollection<Company> _mongoCollection;

    public ProjectRepository(IMongoCollection<Company> mongoCollection)
    {
        _mongoCollection = mongoCollection;
    }
    
    public async Task<ICollection<ProjectDto>> GetCompanyProjectsAsync(string companyId, CancellationToken cancellationToken)
    {
        var filter = Builders<Company>.Filter.Eq(c => c.Id, companyId);
        var companyProjects = await _mongoCollection
            .Find(filter)
            .Project(cu => cu.Projects
                .Select(c => new ProjectDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ProjectStatus = c.ProjectStatus,
                    Timestamp = c.Timestamp,
                }))
            .FirstOrDefaultAsync(cancellationToken);
        
        if (companyProjects == null)
        {
            throw new NotFoundException("Projects are not found");
        }

        return companyProjects.ToList();
    }

    public async Task<ProjectDto> GetCompanyProjectByIdAsync(string companyId, string projectId, CancellationToken cancellationToken)
    {
        var filter = Builders<Company>.Filter.Eq(c => c.Id, companyId);
        var companyProjects = await _mongoCollection
            .Find(filter)
            .Project(cu => cu.Projects
                .Select(c => new ProjectDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ProjectStatus = c.ProjectStatus,
                    Timestamp = c.Timestamp,
                }))
            .FirstOrDefaultAsync(cancellationToken);
        
        if (companyProjects == null)
        {
            throw new NotFoundException("Projects in this company not found");
        }

        var project = companyProjects.FirstOrDefault(c => c.Id == projectId);

        if (project == null)
        {
            throw new NotFoundException("Project not found");
        }
        
        return project;
    }
}