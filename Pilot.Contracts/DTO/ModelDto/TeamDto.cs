using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class TeamDto : BaseDto
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public ICollection<BaseDto> ProjectTasks { get; set; } = new List<BaseDto>();
    
    public ICollection<BaseDto> CompanyUsers { get; set; } = new List<BaseDto>();
}