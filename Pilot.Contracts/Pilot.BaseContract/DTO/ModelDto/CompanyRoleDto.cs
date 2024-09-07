using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Validation.ValidationAttributes;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class CompanyRoleDto : BaseDto
{
    [Required]
    [MaxLength(50)]
    [CheckNameExist]
    public required string Title { get; set; }

    public List<BaseDto> CompanyUsers { get; set; } = [];

    public List<BaseDto> Companies { get; set; } = [];
    
    public bool IsBaseRole { get; init; }
}