using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class CompanyUser : BaseUser
{
    public Company? Company { get; set; }
    
    public Project? Project { get; set; }
    
    public Team? Team { get; set; }

    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    
    public DateTime Timestamp { get; set; } = DateTime.Now;
}