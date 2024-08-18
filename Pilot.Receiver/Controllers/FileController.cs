using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using File = Pilot.Receiver.Models.File;

namespace Pilot.Receiver.Controllers;

public class FileController(IFile repository, ILogger<FileController> logger)
    : BaseReadOnlyController<File, FileDto>(repository, logger);