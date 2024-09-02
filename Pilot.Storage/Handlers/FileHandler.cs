using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Handlers;

public class FileHandler : ModelQueryHandler<File, FileDto>
{
    public FileHandler(IBaseReadRepository<File> repository, ILogger<FileHandler> logger) : base(repository, logger)
    {
    }
}