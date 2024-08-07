﻿using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Consumers.Base;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Consumers.FileConsumer;

public class FileUpdatedConsumer(
    ILogger<FileUpdatedConsumer> logger,
    IFile fileRepository,
    IMessageService message,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<File, FileDto>(logger, fileRepository, message, validate, mapper)
{
}