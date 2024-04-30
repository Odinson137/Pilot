using Pilot.Api.Data.Enums;

namespace Pilot.Contracts.RabbitMqMessages.Project;

public class ProjectUpdated
{
    public string Id { get; set; } = null!;
    public string CompanyId { get; set; } = null!;
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ProjectStatus ProjectStatus { get; set; } 
}