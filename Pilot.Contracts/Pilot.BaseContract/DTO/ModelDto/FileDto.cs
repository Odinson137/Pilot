using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.StorageServer)]
public class FileDto : BaseDto
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    public string? Url { get; set; }
    
    [Required] [MaxLength(30)] public string Type { get; set; } = null!;
    
    [Required] public FileFormat Format { get; set; }
    
    public bool HasNewFile => ByteFormFile != null;
    
    public byte[]? ByteFormFile { get; set; }
    
    public int? UserUploadedId { get; set; }
}