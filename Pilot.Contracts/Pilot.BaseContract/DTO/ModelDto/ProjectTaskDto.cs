using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class ProjectTaskDto : BaseDto
{
    [Required] [MaxLength(50)] public required string Name { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public BaseDto Project { get; set; } = null!;
    
    [Required] public BaseDto CompanyUser { get; set; } = null!;
     
    public BaseDto? File { get; set; }
}