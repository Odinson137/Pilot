using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using File = Pilot.Receiver.Models.File;

namespace Pilot.Receiver.Consumers.FileConsumer;

public class FileDeletedConsumer(
    ILogger<FileDeletedConsumer> logger,
    IFile fileRepository,
    IMessageService message,
    IBaseValidatorService validate)
    : BaseDeleteConsumer<File, FileDto>(logger, fileRepository, message, validate)
{
}