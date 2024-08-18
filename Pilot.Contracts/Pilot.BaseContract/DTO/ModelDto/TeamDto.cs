using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class TeamDto : BaseDto
{
    [Required] [MaxLength(50)] public required string Name { get; set; }

    [Required] [MaxLength(500)] public required string Description { get; set; }

    public ICollection<BaseDto> ProjectTasks { get; set; } = new List<BaseDto>();

    public ICollection<BaseDto> CompanyUsers { get; set; } = new List<BaseDto>();
}