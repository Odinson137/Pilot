using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using TaskStatus = Pilot.Contracts.Data.Enums.TaskStatus;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.WorkerServer)]
public class ProjectTaskDto : BaseDto
{
    [Required] [MaxLength(50)] public required string Name { get; set; }

    [MaxLength(500)] public string? Description { get; set; }

    [Required] public BaseDto Team { get; set; } = null!;

    [Required] public BaseDto CompanyUser { get; set; } = null!;
    
    [Required] public BaseDto CreatedBy { get; set; } = null!;
    
    public List<BaseDto> TaskInfos { get; set; } = [];

    public string? File { get; set; }
    
    public TaskStatus TaskStatus { get; set; }
    
    public TaskPriority Priority { get; set; }
    
    public TimeSpan EstimatedTime { get; set; }
    
    public TimeSpan TimeSpent { get; set; }
}