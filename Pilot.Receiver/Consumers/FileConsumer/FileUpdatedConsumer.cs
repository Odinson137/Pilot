using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using File = Pilot.Receiver.Models.File;

namespace Pilot.Receiver.Consumers.FileConsumer;

public class FileUpdatedConsumer(
    ILogger<FileUpdatedConsumer> logger,
    IFile fileRepository,
    IMessageService message,
    IBaseValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<File, FileDto>(logger, fileRepository, message, validate, mapper)
{
}