using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.CapabilityServer)]
public class CompanyPostDto : BaseDto
{
    [Required] public BaseDto Post { get; set; } = null!;
    
    public ICollection<BaseDto> Applications { get; set; } = [];
    
    [Required] public bool IsOpen { get; set; } = true;
    
    [MaxLength(500)] public string? AdditionalRequirements { get; set; }
    
    public decimal? ExpectedSalary { get; set; }
    
    public int? RequiredExperienceYears { get; set; }
}