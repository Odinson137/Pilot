﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class ProjectViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    public ICollection<TeamViewModel> Teams { get; set; } = new List<TeamViewModel>();

    [Required] public CompanyViewModel Company { get; set; } = null!;
    
    [Required] public CompanyUserViewModel CreatedBy { get; set; } = null!;
    
    public ProjectStatus ProjectStatus { get; set; }
    
    public void AddCompanyUser(CompanyUserViewModel companyUserViewModel)
    {
        CreatedBy = companyUserViewModel;
    }
}