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
    
    [File]
    public string? File { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    public BaseDto? CreatedBy { get; set; }

    public Dictionary<string, ICollection<byte[]>>? Files { get; set; }
}