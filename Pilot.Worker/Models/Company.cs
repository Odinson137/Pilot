using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Worker.Models;

public class Company : BaseModel
{
    [Required] [MaxLength(50)] public string Title { get; init; } = null!;

    [MaxLength(500)] public string? Description { get; init; }

    public ICollection<Project> Projects { get; set; } = [];

    public ICollection<CompanyRole> CompanyRoles { get; set; } = [];
    
    public ICollection<CompanyUser> CompanyUsers { get; set; } = [];

    public CompanyUser? CreatedBy { get; set; }
    
    public string? Logo { get; set; }

    public List<string> InsideImages { get; set; } = [];
}