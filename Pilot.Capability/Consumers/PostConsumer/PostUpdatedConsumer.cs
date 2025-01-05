using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.PostConsumer;

public class PostUpdatedConsumer(
    ILogger<PostUpdatedConsumer> logger,
    IPost repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Post, PostDto>(logger, repository, message, validate, mapper)
{
}