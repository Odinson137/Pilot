using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Consumers.FileConsumer;

public class FileCreatedConsumer(
    ILogger<FileCreatedConsumer> logger,
    IFile fileRepository,
    IMessageService messageService,
    IValidatorService validate,
    IMapper mapper,
    ICompanyUser companyUser)
    : BaseCreatedConsumer<File, FileDto>(logger, fileRepository, messageService, validate, mapper, companyUser)
{
}