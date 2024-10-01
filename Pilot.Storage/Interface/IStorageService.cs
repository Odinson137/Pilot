using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Storage.Interface;

public interface IStorageService
{
    public Task UploadFileAsync(FileDto fileDto);
    
    public Task DeleteFileAsync(string fileName);

    public Task DeleteFolderAsync(string folderName);

    public Task<byte[]> GetFileAsync(string fileName);
    
    public string GetUrl(string fileName, FileFormat format);
}