using Pilot.Api.Data.Enums;

namespace Pilot.Contracts.DTO;

public class ProjectDto
{
    public string Id { get; set; } = null!;
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ProjectStatus ProjectStatus { get; set; } 
    public DateTime Timestamp { get; set; } = DateTime.Now; 
}