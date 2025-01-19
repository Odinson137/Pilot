using AutoMapper;
using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Repository;

public class JobApplicationRepository(DataContext context, IMapper mapper) : BaseRepository<JobApplication>(context, mapper), IJobApplication
{

}