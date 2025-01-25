using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class FileViewModel : BaseViewModel
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    
    [Required] [MaxLength(30)] public string Type { get; set; } = null!;
    
    [Required] public FileFormat Format { get; set; }
    
    public byte[]? ByteFormFile { get; set; }
    
    public int? UserUploadedId { get; set; }
}