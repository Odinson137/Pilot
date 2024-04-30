using Pilot.Contracts.DTO;

namespace Pilot.Receiver.Interface;

public interface IProject
{
    Task<ICollection<ProjectDto>> GetCompanyProjectsAsync(string companyId, CancellationToken cancellationToken);
    Task<ProjectDto> GetCompanyProjectByIdAsync(string companyId, string projectId, CancellationToken cancellationToken);
}