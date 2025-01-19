using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Capability.Data;

public class AutoMapperProfile : BaseMappingProfile
{
    public AutoMapperProfile()
    {
        BaseMap();
        
        Mapping<Post, PostDto>();
        Mapping<Skill, SkillDto>();
        Mapping<UserSkill, UserSkillDto>();
        Mapping<CompanyPost, CompanyPostDto>();
        Mapping<JobApplication, JobApplicationDto>();
    }
}