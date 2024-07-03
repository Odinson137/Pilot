using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Controllers;

public class FileController(IFile repository, ILogger<FileController> logger) : BaseSelectController<File, FileDto>(repository, logger);