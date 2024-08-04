using AutoMapper;
using MassTransit;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models.ModelHelpers;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.Base;

public abstract class BaseCreatedConsumer<T, TDto>(
    ILogger<BaseCreatedConsumer<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : IConsumer<CreateCommandMessage<TDto>>
    where T : BaseModel
    where TDto : BaseDto
{
    protected readonly ILogger<BaseCreatedConsumer<T, TDto>> Logger = logger;
    protected readonly IBaseRepository<T> Repository = repository;
    protected readonly ICompanyUser CompanyUser = companyUser;
    protected readonly IMessageService MessageService = message;
    protected readonly IValidatorService Validator = validate;
    protected readonly IMapper Mapper = mapper;

    public virtual async Task Consume(ConsumeContext<CreateCommandMessage<TDto>> context)
    {
        Logger.LogInformation($"{typeof(T).Name} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<T, TDto>(context.Message.Value, context.Message.UserId);

        var model = Mapper.Map<T>(context.Message.Value);
        
        if (model is IAddCompanyUser addingUserModel)
        {
            var companyUser = await CompanyUser.GetRequiredByIdAsync(context.Message.UserId);
            addingUserModel.AddCompanyUser(companyUser);
        }
        
        await Repository.AddValueToContextAsync(model);

        await Repository.SaveAsync();
        
        var message = new MessageDto
        {
            Title = "Успешное создание!",
            Description = $"Успешное создание сущности '{typeof(T).Name}'",
            MessagePriority = MessagePriority.Success | MessagePriority.Create,
            EntityType = PilotEnumExtensions.GetModelEnumValue<T>()
        };

        await MessageService.SendMessage(message);
    }
}