﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Models;

public class ProjectTask : BaseModel
{
    [Required] [MaxLength(50)] public required string Name { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public Project Project { get; set; } = null!;
    
    [Required] public CompanyUser CompanyUser { get; set; } = null!;
     
    public File? File { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.Now;
}