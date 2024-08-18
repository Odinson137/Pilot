using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Messenger.Consumers.Base;

public abstract class BaseUpdateConsumer<T, TDto>(
    ILogger<BaseUpdateConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : IConsumer<UpdateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseUpdateConsumer<T, TDto>> Logger = logger;
    protected readonly IMapper Mapper = mapper;
    protected readonly IMessageService Message = message;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IBaseValidatorService Validator = validate;

    public virtual async Task Consume(ConsumeContext<UpdateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} update consume");
        Logger.LogClassInfo(context.Message);

        // var dtoModel = context.Message.Value;
        //
        // await Validator.ValidateAsync<T, TDto>(dtoModel, context.Message.UserId);
        //
        // var model = Mapper.Map<T>(dtoModel);
        //
        // await Validator.FillValidateAsync(model);
        // model.ChangeAt = DateTime.Now;
        //
        // Repository.GetContext.Attach(model);
        // Repository.GetContext.Entry(model).State = EntityState.Modified;
        //
        // await Repository.SaveAsync();
        //
        // var message = new MessageDto
        // {
        //     Title = "Успешное обновление!",
        //     Description = $"Успешное обновление сущности {typeof(T).Name}'",
        //     MessagePriority = MessagePriority.Success | MessagePriority.Update,
        //     EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
        //     EntityId = model.Id
        // };
        //
        // await Message.SendMessage(message);
    }
}