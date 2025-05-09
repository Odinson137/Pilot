﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Worker.Models.ModelHelpers;

namespace Pilot.Worker.Models;

public class ProjectTask : BaseModel, IAddCompanyUser
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Required] public TeamEmployee TeamEmployee { get; set; } = null!;

    [Required] public CompanyUser CreatedBy { get; set; } = null!;
    
    public List<TaskInfo> TaskInfos { get; set; } = [];

    public string? File { get; set; }
    
    public ProjectTaskStatus TaskStatus { get; set; }
    
    public TaskPriority Priority { get; set; }

    public TimeSpan EstimatedTime { get; set; }
    
    public TimeSpan TimeSpent { get; set; }
    
    public void AddCompanyUser(CompanyUser companyUser)
    {
        CreatedBy = companyUser;
    }
}