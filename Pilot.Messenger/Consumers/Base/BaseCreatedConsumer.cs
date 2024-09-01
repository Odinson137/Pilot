using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services.LogService;
using Pilot.Messenger.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Messenger.Consumers.Base;

public abstract class BaseCreatedConsumer<T, TDto> : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    // ReSharper disable always MemberCanBePrivate.Global
    protected readonly IBaseValidatorService Validator;
    protected readonly ILogger<BaseCreatedConsumer<T, TDto>> Logger;
    protected readonly IBaseRepository<T> Repository;
    protected readonly IMapper Mapper;
    protected readonly INotificationService NotificationService;

    protected BaseCreatedConsumer(
        ILogger<BaseCreatedConsumer<T, TDto>> logger,
        IBaseRepository<T> repository,
        IBaseValidatorService validatorService,
        IMapper mapper,
        INotificationService notificationService)
    {
        Validator = validatorService;
        Logger = logger;
        Repository = repository;
        Mapper = mapper;
        NotificationService = notificationService;
    }

    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);

        var message = context.Message.Value;
        var userId = context.Message.UserId;

        await Validator.ValidateAsync<T, TDto>(message);

        var model = Mapper.Map<T>(context.Message.Value);

        await Validator.FillValidateAsync(model);

        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();

        Logger.LogInformation($"{typeof(T).Name} consumed");
    }
}