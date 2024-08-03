using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class FileDto : BaseDto
{
    [Required] [MaxLength(50)]
    public required string Url { get; set; }
    
    [Required] [MaxLength(30)] // TODO потом разобраться с этим в валидауии
    public required string Type { get; set; }
}