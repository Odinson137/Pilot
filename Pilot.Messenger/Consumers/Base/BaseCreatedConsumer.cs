using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Messenger.Consumers.Base;

public abstract class BaseCreatedConsumer<T, TDto>(
    ILogger<BaseCreatedConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IBaseValidatorService validatorService,
    IMapper mapper)
    : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    public readonly IBaseValidatorService ValidatorService = validatorService;
    protected readonly ILogger<BaseCreatedConsumer<T, TDto>> Logger = logger;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IMapper Mapper = mapper;

    public virtual Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);

        
        
        Logger.LogInformation($"{typeof(T).Name} consumed");
        return Task.CompletedTask;
    }
}