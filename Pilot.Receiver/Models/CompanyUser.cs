using Pilot.Contracts.Base;

namespace Pilot.Receiver.Models;

public class CompanyUser : BaseModel
{
    public Company? Company { get; set; }

    public Project? Project { get; set; }

    public Team? Team { get; set; }

    public List<ProjectTask> ProjectTasks { get; set; } = [];
    
    public List<CompanyRole> CompanyRoles { get; set; } = [];
}