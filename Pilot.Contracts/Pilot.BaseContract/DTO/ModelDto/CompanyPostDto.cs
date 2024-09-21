using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Validation.ValidationAttributes;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class CompanyPostDto : BaseDto
{
    public int? CompanyUserId { get; set; } // если пусто, значит вакансия может быть открыта

    public bool IsOpen { get; set; } = true;

    [Required] public BaseDto Post { get; set; } = null!;
    
    [MaxLength(500)] public string? Description { get; set; }
}