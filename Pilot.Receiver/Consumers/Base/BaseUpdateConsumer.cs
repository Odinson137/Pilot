using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.Base;

public abstract class BaseUpdateConsumer<T, TDto>(
    ILogger<BaseUpdateConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : IConsumer<UpdateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseUpdateConsumer<T, TDto>> Logger = logger;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly IMessageService Message = message;
    protected readonly IValidatorService Validator = validate;
    protected readonly IMapper Mapper = mapper;

    public virtual async Task Consume(ConsumeContext<UpdateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} update consume");
        Logger.LogClassInfo(context.Message);

        var dtoModel = context.Message.Value;
        
        await Validator.ValidateAsync<T, TDto>(dtoModel, context.Message.UserId);

        var model = Mapper.Map<T>(dtoModel);

        await Validator.UpdateValidateAsync(model);
        model.ChangeAt = DateTime.Now;
        
        Repository.GetContext.Attach(model);
        Repository.GetContext.Entry(model).State = EntityState.Modified;

        await Repository.SaveAsync();

        var message = new MessageDto
        {
            Title = "Успешное обновление!",
            Description = $"Успешное обновление сущности {typeof(T).Name}'",
            MessagePriority = MessagePriority.Success | MessagePriority.Update,
            EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
            EntityId = model.Id
        };

        await Message.SendMessage(message);
    }
}