using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.StorageServer)]
public class FileDto : BaseDto
{
    public bool HasNewFile => ByteFormFile != null;
  
    // TODO потом придумать что с этим делать: возможно в каждом клиенте сразу кидать файлы в виде байтов
    // [JsonIgnore]
    // public IFormFile? FormFile { get; set; }
    
    public byte[]? ByteFormFile { get; set; }

    [Required]
    [MaxLength(30)] // TODO потом разобраться с этим в валидауии
    public string Type { get; set; } = null!;

    public double GetSize() =>
        ByteFormFile?.Length / 1024.0 ?? throw new System.Exception("Нет файла, чтоб узнать его длину");
}