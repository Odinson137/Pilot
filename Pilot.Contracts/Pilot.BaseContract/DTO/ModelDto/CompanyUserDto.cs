using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class CompanyUserDto : BaseDto
{
    public BaseDto? Company { get; set; }

    public List<BaseDto> Team { get; set; } = [];

    public List<BaseDto> ProjectTasks { get; set; } = [];
    
    public BaseDto? CompanyRole { get; set; }
}
