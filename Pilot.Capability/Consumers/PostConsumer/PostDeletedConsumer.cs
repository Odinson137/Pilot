using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.PostConsumer;

public class PostDeletedConsumer(
    ILogger<PostDeletedConsumer> logger,
    IPost repository,
    IMessageService message,
    IValidatorService validate)
    : BaseDeleteConsumer<Post, PostDto>(logger, repository, message, validate)
{
}