﻿using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class TaskInfoViewModel : BaseViewModel
{
    [Required] public ProjectTaskViewModel ProjectTask { get; set; } = null!;

    public string? File { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    public CompanyUserViewModel? CreatedBy { get; set; }
    
    public TimeSpan TimeSpent { get; set; }
}