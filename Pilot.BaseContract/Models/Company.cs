using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models.ModelHelpers;

namespace Pilot.Contracts.Models;

public class Company : BaseModel, IAddCompanyUser
{
    [Required] [MaxLength(50)] public string Title { get; init; } = null!;
    [MaxLength(500)] public string? Description { get; init; }
    
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    
    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();

    public void AddCompanyUser(CompanyUser companyUser)
    {
        CompanyUsers.Add(companyUser);
    }
}

