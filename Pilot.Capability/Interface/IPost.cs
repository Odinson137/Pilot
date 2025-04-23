using Pilot.Capability.Models;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Interface;

public interface IPost : IBaseRepository<Post>
{
    public Task<Post> GetPostIncludeSkillsAsync(int skillId, CancellationToken token);
}