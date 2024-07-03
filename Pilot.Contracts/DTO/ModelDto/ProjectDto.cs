using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class ProjectDto : BaseDto
{
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public ICollection<BaseDto> Teams { get; set; } = new List<BaseDto>();
    
    public ICollection<BaseDto> ProjectTasks { get; set; } = new List<BaseDto>();
    
    public ICollection<BaseDto> CompanyUsers { get; set; } = new List<BaseDto>();
    
    public ProjectStatus ProjectStatus { get; set; } 
    
    public DateTime Timestamp { get; set; } = DateTime.Now; 
}