using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Storage.Interface;

public interface IStorageService
{
    public Task UploadOrChangeFileAsync(FileDto fileDto);
    
    public Task DeleteFileAsync(int id);

    public Task<byte[]> GetFileAsync(int id);
    
    public Task<string> GetUrlAsync(int id);
}