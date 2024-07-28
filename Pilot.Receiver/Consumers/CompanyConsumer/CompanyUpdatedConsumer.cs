using AutoMapper;
using MassTransit;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using Pilot.Contracts.RabbitMqMessages;
using Pilot.Contracts.Services.LogService;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Consumers.CompanyConsumer;

public class CompanyUpdatedConsumer(
    ILogger<CompanyUpdatedConsumer> logger,
    ICompany company,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<Company, CompanyDto>(logger, company, message, validate, mapper)
{
}