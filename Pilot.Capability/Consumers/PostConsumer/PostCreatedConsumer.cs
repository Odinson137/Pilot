﻿using AutoMapper;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.Capability.Consumers.PostConsumer;

public class PostCreatedConsumer(
    ILogger<PostCreatedConsumer> logger,
    IPost repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper)
    : BaseCreatedConsumer<Post, PostDto>(logger, repository, messageService, validate, mapper)
{
}