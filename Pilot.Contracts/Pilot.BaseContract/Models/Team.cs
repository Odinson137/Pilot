﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models.ModelHelpers;

namespace Pilot.Contracts.Models;

public class Team : BaseModel, IAddCompanyUser
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [Required] [MaxLength(500)] public string Description { get; set; } = null!;
    
    public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CompanyUsers.Add(companyUser);
    }
}