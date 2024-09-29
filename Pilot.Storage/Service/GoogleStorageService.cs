using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Storage.Interface;

namespace Pilot.Storage.Service;

public class GoogleStorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly string _cloudUrl;
    private readonly StorageClient _storageClient;

    public GoogleStorageService(IConfiguration configuration)
    {
        var credential = configuration.GetValue<string>("Credential")!;
        var googleCredential = GoogleCredential.FromFile(credential);
        _storageClient = StorageClient.Create(googleCredential);
        _cloudUrl = configuration.GetValue<string>("GoogleUrl")!;
        _bucketName = configuration.GetValue<string>("BucketName")!;
    }
    
    public async Task UploadFileAsync(FileDto fileDto)
    {
        using var memoryStream = new MemoryStream(fileDto.ByteFormFile!);
        var formatType = fileDto.Format.ToString().ToLower();
        var fullUrl = $"{formatType}/{fileDto.Name}";
        await _storageClient.UploadObjectAsync(_bucketName, fullUrl, $"{formatType}/{fileDto.Type}", memoryStream);
    }

    public async Task DeleteFileAsync(string fileName)
    {
        await _storageClient.DeleteObjectAsync(_bucketName, fileName);
    }
    
    public async Task DeleteFolderAsync(string folderName)
    {
        var awaitList = new List<Task>();
        
        foreach (var storageObject in _storageClient.ListObjects(_bucketName, folderName))
        {
            var task = _storageClient.DeleteObjectAsync(_bucketName, storageObject.Name);
            awaitList.Add(task);
            Console.WriteLine($"Deleted {storageObject.Name}.");
        }

        await Task.WhenAll(awaitList);
    }

    public async Task<byte[]> GetFileAsync(string fileName)
    {
        using var memoryStream = new MemoryStream();
        await _storageClient.DownloadObjectAsync(_bucketName, fileName, memoryStream);
        return memoryStream.ToArray();
    }

    public string GetUrl(string fileName, FileFormat format, string type)
    {
        return $"{_cloudUrl}/{_bucketName}/{type}/{fileName}.{format}";
    }
}