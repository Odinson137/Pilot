using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Validation.ValidationAttributes;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class CompanyDto : BaseDto, IHasFile
{
    [Required]
    [MaxLength(50)]
    [CheckNameExist]
    public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; init; }

    public ICollection<BaseDto> Projects { get; init; } = new List<BaseDto>();

    public ICollection<BaseDto> CompanyRoles { get; set; } = new List<BaseDto>();
    
    [HasFile(nameof(LogoUrl))]
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public int? LogoId { get; set; }

    public string? LogoUrl { get; set; }

    [HasFile(nameof(InsideImagesUrl))]
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public ICollection<int> InsideImagesId { get; set; } = [];

    public ICollection<string> InsideImagesUrl { get; set; } = [];
}