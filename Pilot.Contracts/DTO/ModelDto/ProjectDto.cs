using System.ComponentModel.DataAnnotations;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class ProjectDto : BaseDto
{
    [Required] [MaxLength(100)] public required string Name { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }
    
    public ICollection<BaseDto> Teams { get; set; } = new List<BaseDto>();
    
    public ICollection<BaseDto> ProjectTasks { get; set; } = new List<BaseDto>();
    
    public ICollection<BaseDto> CompanyUsers { get; set; } = new List<BaseDto>();
    
    public ProjectStatus ProjectStatus { get; set; } 
}