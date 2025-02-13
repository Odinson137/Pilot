using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.DTO.ModelDto;

[FromService(ServiceName.WorkerServer)]
public class TeamEmployeeDto : BaseDto
{
    public BaseDto Team { get; set; }

    public BaseDto CompanyUser { get; set; }
}