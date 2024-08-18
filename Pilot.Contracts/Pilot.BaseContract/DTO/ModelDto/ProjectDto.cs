using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class ProjectDto : BaseDto
{
    [Required] [MaxLength(100)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    public ICollection<BaseDto> Teams { get; set; } = new List<BaseDto>();

    public ICollection<BaseDto> ProjectTasks { get; set; } = new List<BaseDto>();

    public ICollection<BaseDto> CompanyUsers { get; set; } = new List<BaseDto>();

    public ProjectStatus ProjectStatus { get; set; }
}