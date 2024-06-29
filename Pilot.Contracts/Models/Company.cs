using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class Company : BaseModel
{
    [Required] [MaxLength(50)] public required string Title { get; init; } = null!;
    [MaxLength(500)] public string? Description { get; init; }
    
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    
    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
}

