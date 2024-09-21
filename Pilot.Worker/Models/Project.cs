using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Worker.Models.ModelHelpers;

namespace Pilot.Worker.Models;

public class Project : BaseModel, IAddCompanyUser
{
    [Required] [MaxLength(100)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    public ICollection<Team> Teams { get; set; } = new List<Team>();

    [Required] public Company Company { get; set; } = null!;
    
    [Required] public CompanyUser CreatedBy { get; set; } = null!;
    
    public ProjectStatus ProjectStatus { get; set; }
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CreatedBy = companyUser;
    }
}