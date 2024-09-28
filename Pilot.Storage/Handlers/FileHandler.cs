using MediatR;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.Storage.Interface;
using Pilot.Storage.Queries;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Handlers;

public class FileHandler : 
    ModelQueryHandler<File, FileDto>, 
    IRequestHandler<GetFileUrlQuery<FileDto>, FileDto>,
    IRequestHandler<GetFileUrlsQuery<FileDto>, ICollection<FileDto>>
{
    private readonly IFileService _fileService;
    public FileHandler(IFileRepository repository, ILogger<FileHandler> logger, IFileService fileService) : base(repository, logger)
    {
        _fileService = fileService;
    }
    
    public async Task<FileDto> Handle(GetFileUrlQuery<FileDto> request, CancellationToken cancellationToken)
    {
        var result = await _fileService.GetUrlAsync(request.Id);
        return result;
    }

    public async Task<ICollection<FileDto>> Handle(GetFileUrlsQuery<FileDto> request, CancellationToken cancellationToken)
    {
        var result = await _fileService.GetUrlsAsync(request.Filter);
        return result;
    }
}