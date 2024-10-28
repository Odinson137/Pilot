using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Storage.Interface;

public interface IFileService
{
    public Task<int> UploadFileAsync(FileDto fileDto);
    
    public Task ChangeFileAsync(FileDto fileDto);

    public Task DeleteFileAsync(int id);

    public Task<FileDto> GetFileAsync(int id);

    public Task<FileDto> GetUrlAsync(int id);

    Task<ICollection<FileDto>> GetUrlsAsync(BaseFilter? filter);
}