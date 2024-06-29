using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class ProjectTaskDto : BaseModel
{
    public required string Name { get; set; }
    
    public string? Description { get; set; }

    public BaseDto Project { get; set; } = null!;
    
    public BaseDto CompanyUserDto { get; set; } = null!;
     
    public BaseDto? File { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.Now;
}