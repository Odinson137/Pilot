using AutoMapper;
using MassTransit;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers;

public class CompanyDeletedConsumer(
    ILogger<CompanyDeletedConsumer> logger,
    ICompany company,
    IMessage message,
    IValidateService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseDeleteConsumer<Company, CompanyDto>(logger, company, message, validate, mapper, companyUser)
{
    // public override async Task Consume(ConsumeContext<DeleteCommandMessage<CompanyDto>> context)
    // {
    //
    // }
}