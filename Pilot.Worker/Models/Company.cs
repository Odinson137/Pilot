﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Worker.Models;

public class Company : BaseModel
{
    [Required] [MaxLength(50)] public string Title { get; init; } = null!;

    [MaxLength(500)] public string? Description { get; init; }

    public List<Project> Projects { get; set; } = [];

    public List<CompanyRole> CompanyRoles { get; set; } = [];
    
    public ICollection<CompanyUser> CompanyUsers { get; set; } = [];
    
    public int? LogoId { get; set; }

    public List<string> InsideImages { get; set; } = [];
}