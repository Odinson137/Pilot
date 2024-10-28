using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Interfaces;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.WorkerServer)]
public class TaskInfoDto : BaseDto, IHasFile
{
    [Required] public BaseDto ProjectTask { get; set; } = null!;
    
    [HasFile(nameof(FileUrl))]
    public int? FileId { get; set; }

    public string? FileUrl { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [Required] public BaseDto CreatedBy { get; set; } = null!;
}