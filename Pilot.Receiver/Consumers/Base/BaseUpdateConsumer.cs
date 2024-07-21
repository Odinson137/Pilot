using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.Base;

public abstract class BaseUpdateConsumer<T, TDto>(
    ILogger<BaseUpdateConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessage message,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : IConsumer<UpdateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseUpdateConsumer<T, TDto>> Logger = logger;
    protected readonly IBaseRepository<T> Repository = repository;
    protected  readonly ICompanyUser CompanyUser = companyUser;
    protected  readonly IMessage Message = message;
    protected readonly IValidatorService Validator = validate;
    protected  readonly IMapper Mapper = mapper;

    public virtual async Task Consume(ConsumeContext<UpdateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} update consume");
        Logger.LogClassInfo(context.Message);

        await Validator.Validate<T, TDto>(context.Message.Value, context.Message.UserId);

        var model = Mapper.Map<T>(context.Message.Value);

        await Validator.UpdateValidate(model);
        model.ChangeAt = DateTime.Now;
        
        Repository.GetContext.Attach(model);
        Repository.GetContext.Entry(model).State = EntityState.Modified;

        await Repository.SaveAsync();

        var asd = await Repository.GetContext.Set<T>().ToListAsync();
        
        await Message.SendMessage("Успешное обновление!",
            $"Успешное обновление сущности {typeof(T).Name}'",
            MessagePriority.Success | MessagePriority.Update);
    }
}