using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Storage.Interface;

namespace Pilot.Storage.Service;

public class GoogleStorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly StorageClient _storageClient;
    private readonly IFileRepository _fileRepository;

    private const string Credential = "pilot.Credential.GoogleStorage.json";

    public GoogleStorageService(IConfiguration configuration, IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
        var googleCredential = GoogleCredential.FromFile(Credential);
        _bucketName = configuration.GetValue<string>("BucketName")!;
        _storageClient = StorageClient.Create(googleCredential);
    }
    
    public async Task UploadOrChangeFileAsync(FileDto fileDto)
    {
        var fileModel = fileDto.Id != 0 ? await _fileRepository.GetByIdAsync(fileDto.Id) : null;

        var fileName = Guid.NewGuid().ToString();
        
        using var memoryStream = new MemoryStream(fileDto.ByteFormFile!);
        
        await _storageClient.UploadObjectAsync(_bucketName, fileName, $"image/{fileDto.Type}", memoryStream);

        if (fileModel != null)
        {
            fileModel.Name = fileName;
            fileModel.Size = fileDto.GetSize();
            fileModel.ChangeAt = DateTime.Now;
            await _fileRepository.SaveAsync();
            
            await _storageClient.DeleteObjectAsync(_bucketName, fileModel.Name);
        }
    }

    public async Task DeleteFileAsync(int id)
    {
        await _fileRepository.FastDeleteAsync(id);
        await _storageClient.DeleteObjectAsync(_bucketName, Guid.NewGuid().ToString());
    }
    
    public async Task<byte[]> GetFileAsync(int id)
    {
        var fileModel = await _fileRepository.GetRequiredByIdAsync(id);

        using var memoryStream = new MemoryStream();
        await _storageClient.DownloadObjectAsync(_bucketName, fileModel.Name, memoryStream);
        return memoryStream.ToArray();
    }

    public async Task<string> GetUrlAsync(int id)
    {
        var fileModel = await _fileRepository.GetRequiredByIdAsync(id);
        return fileModel.Url;
    }
}