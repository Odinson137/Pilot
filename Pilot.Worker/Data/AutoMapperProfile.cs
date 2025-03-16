using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;

namespace Pilot.Worker.Data;

public class AutoMapperProfile : BaseMappingProfile
{
    public AutoMapperProfile()
    {
        BaseMap();

        Mapping<CompanyUser, CompanyUserDto>();

        Mapping<CompanyUser, UserDto>();

        Mapping<Project, ProjectDto>();
        Mapping<ProjectTask, ProjectTaskDto>();
        Mapping<Team, TeamDto>();
        Mapping<CompanyRole, CompanyRoleDto>();
        Mapping<TaskInfo, TaskInfoDto>();
        Mapping<TeamEmployee, TeamEmployeeDto>();
    }
}