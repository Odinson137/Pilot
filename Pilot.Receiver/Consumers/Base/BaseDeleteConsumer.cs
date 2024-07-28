using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.Base;

public abstract class BaseDeleteConsumer<T, TDto>(
    ILogger<BaseDeleteConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : IConsumer<DeleteCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseDeleteConsumer<T, TDto>> Logger = logger;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly ICompanyUser CompanyUser = companyUser;
    protected readonly IMessageService MessageService = messageService;
    protected readonly IValidatorService Validator = validate;
    protected readonly IMapper Mapper = mapper;

    public virtual async Task Consume(ConsumeContext<DeleteCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} delete consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateWithoutDefaultAsync<T, TDto>(context.Message.Value, context.Message.UserId);
        
        var model = Mapper.Map<T>(context.Message.Value);
        
        await Validator.DeleteValidateAsync(model);
        
        Repository.DeleteAsync(model);

        await Repository.SaveAsync();
        
        var message = new MessageDto
        {
            Title = "Успешное удаление!",
            Description =  $"Успешное удаление сущности {typeof(T).Name}'",
            MessagePriority = MessagePriority.Success | MessagePriority.Delete,
            EntityType = typeof(T).ToString(),
        };
            
        await MessageService.SendMessage(message);
    }
}