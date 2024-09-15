using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Capability.Handlers;

public class PostQueryHandler : ModelQueryHandler<Post, PostDto>
{
    public PostQueryHandler(IPost repository, ILogger<PostQueryHandler> logger) : base(repository, logger)
    {
    }
}