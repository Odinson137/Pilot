using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.WorkerServer)]
public class CompanyUserDto : BaseDto
{
    public BaseDto? Company { get; set; }

    public int PostId { get; set; }

    public List<BaseDto> Teams { get; set; } = [];

    public List<BaseDto> ProjectTasks { get; set; } = [];
    
    public BaseDto? CompanyRole { get; set; }
}
