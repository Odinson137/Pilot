using AutoMapper;
using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Repository;

public class PostRepository(DataContext context, IMapper mapper) : BaseRepository<Post>(context, mapper), IPost
{

}