using AutoMapper;
using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Repository;

public class SkillRepository(DataContext context, IMapper mapper) : BaseRepository<Skill>(context, mapper), ISkill
{

}