using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Messenger.Consumers.Base;

public abstract class BaseCreatedConsumer<T, TDto> : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly IBaseValidatorService Validator;
    protected readonly ILogger<BaseCreatedConsumer<T, TDto>> Logger;
    protected readonly IBaseRepository<T> Repository;
    protected readonly IMapper Mapper;

    protected BaseCreatedConsumer(ILogger<BaseCreatedConsumer<T, TDto>> logger,
        IBaseRepository<T> repository,
        IBaseValidatorService validatorService,
        IMapper mapper)
    {
        Validator = validatorService;
        Logger = logger;
        Repository = repository;
        Mapper = mapper;
    }

    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<T, TDto>(context.Message.Value, context.Message.UserId);

        var model = Mapper.Map<T>(context.Message.Value);

        await Validator.FillValidateAsync(model);
        
        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        // await HubContext.Clients.User();
        
        Logger.LogInformation($"{typeof(T).Name} consumed");
    }
}