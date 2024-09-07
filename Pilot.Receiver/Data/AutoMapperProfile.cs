using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Data;

public class AutoMapperProfile : BaseMappingProfile
{
    public AutoMapperProfile()
    {
        BaseMap();
        
        Mapping<Company, CompanyDto>();
        Mapping<CompanyUser, CompanyUserDto>();
        
        Mapping<CompanyUser, UserDto>();

        Mapping<Project, ProjectDto>();
        Mapping<ProjectTask, ProjectTaskDto>();
        Mapping<Team, TeamDto>();
        Mapping<HistoryAction, HistoryActionDto>();
        Mapping<CompanyRole, CompanyRoleDto>();
        Mapping<TaskInfo, TaskInfoDto>();
    }
}