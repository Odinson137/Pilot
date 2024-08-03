using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.Models;

public class Project : BaseModel
{
    [Required] [MaxLength(100)] public string Name { get; set; } = null!;
    
    [MaxLength(500)] public string? Description { get; set; }
    
    public ICollection<Team> Teams { get; set; } = new List<Team>();
    
    public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    
    public ProjectStatus ProjectStatus { get; set; } 
}