using AutoMapper;
using MassTransit;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using IMessage = Pilot.Receiver.Interface.IMessage;

namespace Pilot.Receiver.Consumers;

public class CompanyCreatedConsumer(
    ILogger<CompanyCreatedConsumer> logger,
    ICompany company,
    IMessage message,
    IValidateService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<Company, CompanyDto>(logger, company, message, validate, mapper, companyUser)
{
    public override async Task Consume(ConsumeContext<CreateCommandMessage<CompanyDto>> context)
    {
        Logger.LogInformation("Company create consume");
        Logger.LogClassInfo(context.Message);

        await Validator.Validate<Company, CompanyDto>(context.Message.Value, context.Message.UserId);

        var companyUser = await CompanyUser.GetByIdAsync(context.Message.UserId);
        
        await Repository.AddNewValueAsync(new Company
        {
            Title = context.Message.Value.Title,
            Description = context.Message.Value.Title,
            CompanyUsers = new List<CompanyUser> { companyUser! }
        });

        await Repository.SaveAsync();
        
        await Message.SendMessage("Компания создана!",
            $"Успешное создание компании с названием '{context.Message.Value.Title}'",
            MessagePriority.Success | MessagePriority.Create);
    }
}