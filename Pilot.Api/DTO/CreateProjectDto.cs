using System.ComponentModel.DataAnnotations;
using Pilot.Api.Data.Enums;

namespace Pilot.Api.DTO;

public class CreateProjectDto
{
    [Required]
    [MinLength(5)]
    public string CompanyId { get; set; } = null!;
    [Required]
    [MinLength(3)]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ProjectStatus ProjectStatus { get; set; } 
}