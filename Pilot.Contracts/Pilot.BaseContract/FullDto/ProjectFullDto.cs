using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.FullDto;

[FromService(ServiceName.WorkerServer)]
public class ProjectFullDto : BaseDto
{
    [Required] [MaxLength(100)] public string Name { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    public ICollection<TeamFullDto> Teams { get; set; } = [];

    public CompanyFullDto Company { get; set; } = null!;
    
    public CompanyUserFullDto? CreatedBy { get; set; }

    public ProjectStatus ProjectStatus { get; set; }
}