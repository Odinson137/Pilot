using Pilot.Contracts.Base;

namespace Pilot.Contracts.DTO.ModelDto;

public class CompanyUserDto : BaseUserDto
{
    public BaseDto? Company { get; set; }
    
    public BaseDto? Project { get; set; }
    
    public BaseDto? Team { get; set; }

    public ICollection<BaseDto> Tasks { get; set; } = new List<BaseDto>();
}