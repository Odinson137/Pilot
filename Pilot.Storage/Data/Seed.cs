using Bogus;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Storage.Interface;

namespace Pilot.Storage.Data;

public class Seed : ISeed
{
    private readonly IFileService _fileService;
    private readonly DataContext _context;
    private readonly IStorageService _storageService;

    public Seed(IFileService fileService, DataContext context, IStorageService storageService)
    {
        _fileService = fileService;
        _context = context;
        _storageService = storageService;
    }

    public async Task Seeding()
    {
        if (await _context.Files.AnyAsync()) return;
        
        await _storageService.DeleteFolderAsync(FileFormat.Image.ToString().ToLower());
        
        var imagesDictionary = GetFilesFromDirectory(
            "wwwroot/SeedImages/UserProfileLogos", // count - 30
            "wwwroot/SeedImages/CompanyLogos", // count - 5
            "wwwroot/SeedImages/CompanyInsides" // count - 50
            );

        if (imagesDictionary.Count < Constants.SeedDataCount) throw new Exception("Данных в сиде меньше, чем должно быть");

        var fileFaker = GetFileDtoFaker();
        
        foreach (var (fileNamePart, value) in imagesDictionary)
        {
            var fileType = fileNamePart.Split(".")[1];
            var fileDto = fileFaker.Generate();
            fileDto.Type = fileType;
            fileDto.ByteFormFile = value;

            await _fileService.UploadFileAsync(fileDto);
        }
    }

    private Faker<FileDto> GetFileDtoFaker()
    {
        var fakeUser = new Faker<FileDto>()
            .RuleFor(u => u.Format, (_, _) => FileFormat.Image)
            ;
        
        return fakeUser;
    }
    
    private static Dictionary<string, byte[]> GetFilesFromDirectory(params string[] directoryPaths)
    {
        var filesDictionary = new Dictionary<string, byte[]>();

        foreach (var directoryPath in directoryPaths)
        {
            var filePaths = Directory.GetFiles(directoryPath);

            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileName(filePath);

                var fileContent = File.ReadAllBytes(filePath);

                if (filesDictionary.TryGetValue(fileName, out _)) throw new Exception("Упс.. Ты запихнул файл, название которое уже где-то есть. Change it");
                filesDictionary[fileName] = fileContent;
            }
        }

        return filesDictionary;
    }
}