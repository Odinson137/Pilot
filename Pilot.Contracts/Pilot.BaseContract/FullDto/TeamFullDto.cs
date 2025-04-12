using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.FullDto;

[FromService(ServiceName.WorkerServer)]
public class TeamFullDto : BaseDto
{
    [Required] [MaxLength(50)] public required string Name { get; set; }

    [Required] [MaxLength(500)] public required string Description { get; set; }

    public ICollection<CompanyUserFullDto> CompanyUsers { get; set; } = [];

    [Required] public ProjectFullDto Project { get; set; } = null!;
}