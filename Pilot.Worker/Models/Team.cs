using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Worker.Models.ModelHelpers;

namespace Pilot.Worker.Models;

public class Team : BaseModel, IAddCompanyUser
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [Required] [MaxLength(500)] public string Description { get; set; } = null!;

    public List<CompanyUser> CompanyUsers { get; set; } = [];

    [Required] public Project Project { get; set; } = null!;
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CompanyUsers.Add(companyUser);
    }
}