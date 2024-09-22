using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.WorkerServer)]
public class ProjectDto : BaseDto
{
    [Required] [MaxLength(100)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    public ICollection<BaseDto> Teams { get; set; } = new List<BaseDto>();

    public BaseDto Company { get; set; } = null!;
    
    public BaseDto CreatedBy { get; set; } = null!;

    public ProjectStatus ProjectStatus { get; set; }
}