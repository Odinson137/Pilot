using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Data;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Repository;

public class PostRepository(DataContext context, IMapper mapper) : BaseRepository<Post>(context, mapper), IPost
{
    public async Task<Post> GetPostIncludeSkillsAsync(int skillId, CancellationToken token)
    {
        return await context.Posts.Include(c => c.Skills).SingleAsync(c => c.Id == skillId, token);
    }
}