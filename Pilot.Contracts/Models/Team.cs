using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class Team : BaseModel
{
    [Required] [MaxLength(50)] public required string Name { get; set; }
    
    [Required] [MaxLength(500)] public required string Description { get; set; }
    
    public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
}