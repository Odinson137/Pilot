
using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class CompanyDto : BaseDto
{
    public required string Title { get; init; }

    public string? Description { get; init; }

    public ICollection<BaseDto> Projects { get; init; } = new List<BaseDto>();
    
    public ICollection<BaseDto> Teams { get; init; } = new List<BaseDto>();
    
    public ICollection<BaseDto> CompanyUsers { get; init; } = new List<BaseDto>();
}