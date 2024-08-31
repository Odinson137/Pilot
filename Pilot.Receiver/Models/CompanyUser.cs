using Pilot.Contracts.Base;

namespace Pilot.Receiver.Models;

public class CompanyUser : BaseModel
{
    public Company? Company { get; set; }

    public Project? Project { get; set; }

    public Team? Team { get; set; }

    public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
}