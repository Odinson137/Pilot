using AutoMapper;
using MassTransit;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Consumers.Base;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;
using PilotEnumExtensions = Pilot.Contracts.Services.PilotEnumExtensions;

namespace Pilot.Worker.Consumers.CompanyConsumer;

public class CompanyCreatedConsumer(
    ILogger<CompanyCreatedConsumer> logger,
    ICompany company,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Company, CompanyDto>(logger, company, messageService, validate, mapper, companyUser)
{
    public override async Task Consume(ConsumeContext<CreateCommandMessage<CompanyDto>> context)
    {
        Logger.LogInformation($"{nameof(Company)} create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.ValidateAsync<Company, CompanyDto>(context.Message.Value);

        var model = Mapper.Map<Company>(context.Message.Value);

        await Validator.FillValidateAsync(model);

        await Repository.AddValueToContextAsync(model);

        var firstEmployee = new CompanyUser
        {
            Id = context.Message.UserId,
            Company = model,
        };

        await companyUser.AddValueToContextAsync(firstEmployee);

        await Repository.SaveAsync();

        var message = new InfoMessageDto
        {
            Title = "Успешное создание!",
            Description = $"Успешное создание компании '{model.Title}'",
            MessagePriority = MessageInfo.Success | MessageInfo.Create,
            EntityType = PilotEnumExtensions.GetModelEnumValue<Company>(),
            EntityId = model.Id
        };

        await MessageService.SendInfoMessageAsync(message, context.Message.UserId);
    }
}