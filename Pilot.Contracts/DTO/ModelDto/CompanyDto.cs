
using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Validation.ValidationAttributes;

namespace Pilot.Contracts.DTO.ModelDto;

public class CompanyDto : BaseDto
{
    [Required] [MaxLength(50)] [CheckNameExist]
    public required string Title { get; set; }

    [MaxLength(500)] 
    public string? Description { get; init; }

    public ICollection<BaseDto> Projects { get; init; } = new List<BaseDto>();
    
    public ICollection<BaseDto> Teams { get; init; } = new List<BaseDto>();
    
    public ICollection<BaseDto> CompanyUsers { get; init; } = new List<BaseDto>();
}