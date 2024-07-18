using System.ComponentModel.DataAnnotations;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class Project : BaseModel
{
    [Required] [MaxLength(100)] public required string Name { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }
    
    public ICollection<Team> Teams { get; set; } = new List<Team>();
    
    public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    
    public ProjectStatus ProjectStatus { get; set; } 
}