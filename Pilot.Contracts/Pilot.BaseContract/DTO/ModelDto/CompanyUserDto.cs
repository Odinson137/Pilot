using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.ReceiverServer)]
public class CompanyUserDto : BaseDto
{
    public BaseDto? Company { get; set; }

    public BaseDto? Project { get; set; }

    public BaseDto? Team { get; set; }

    public ICollection<BaseDto> Tasks { get; set; } = new List<BaseDto>();
}