using System.ComponentModel.DataAnnotations;
using Pilot.Api.Data.Enums;

namespace Pilot.Api.DTO;

public class UpdateProjectDto
{
    [Required]
    [MinLength(5)]
    public string Id { get; set; } = null!;
    [Required]
    [MinLength(5)]
    public string CompanyId { get; set; } = null!;
    [Required]
    [MinLength(3)]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ProjectStatus ProjectStatus { get; set; } 
}