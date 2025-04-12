using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Contracts.FullDto;

[FromService(ServiceName.WorkerServer)]
public class CompanyUserFullDto : BaseDto
{
    public CompanyFullDto? Company { get; set; }

    public int PostId { get; set; }

    public List<TeamFullDto> Teams { get; set; } = [];

    public List<ProjectFullDto> ProjectTasks { get; set; } = [];
}
