﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class ProjectTaskViewModel : BaseViewModel
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    // [Required] public TeamViewModel Team { get; set; } = null!;
    //
    // public CompanyUserViewModel? CompanyUser { get; set; }

    public TeamEmployeeViewModel? TeamEmployee { get; set; }
    
    [Required] public CompanyUserViewModel CreatedBy { get; set; } = null!;
    
    public ICollection<TaskInfoViewModel> TaskInfos { get; set; } = [];

    public int? FileId { get; set; }
    
    public ProjectTaskStatus TaskStatus { get; set; }
    
    public TaskPriority Priority { get; set; }
    
    public TimeSpan EstimatedTime { get; set; }
    
    public TimeSpan TimeSpent { get; set; }
}