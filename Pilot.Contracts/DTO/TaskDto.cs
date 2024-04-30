namespace Pilot.Contracts.DTO;

public class TaskDto
{
    private string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public FileDto? File { get; set; }
    public DateTime Timestamp { get; set; }
}