using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Storage.Consumers.Base;

public abstract class BaseCreatedConsumer<T, TDto> : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    // ReSharper disable always MemberCanBePrivate.Global
    protected readonly IBaseValidatorService Validator;
    protected readonly ILogger<BaseCreatedConsumer<T, TDto>> Logger;
    protected readonly IBaseRepository<T> Repository;
    protected readonly IMapper Mapper;
    protected readonly IMessageService MessageService;

    protected BaseCreatedConsumer(
        ILogger<BaseCreatedConsumer<T, TDto>> logger,
        IBaseRepository<T> repository,
        IBaseValidatorService validatorService,
        IMapper mapper,
        IMessageService messageService)
    {
        Validator = validatorService;
        Logger = logger;
        Repository = repository;
        Mapper = mapper;
        MessageService = messageService;
    }

    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);
    }
}