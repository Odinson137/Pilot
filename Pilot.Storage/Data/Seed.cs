using AutoMapper;
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
    private readonly IMapper _mapper;

    public Seed(IFileService fileService, DataContext context, IMapper mapper)
    {
        _fileService = fileService;
        _context = context;
        _mapper = mapper;
    }

    public async Task Seeding()
    {
        if (await _context.Files.AnyAsync()) return;
        
        var imagesDictionary = GetFilesFromDirectory("wwwroot/SeedImages");

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
    
    private static Dictionary<string, byte[]> GetFilesFromDirectory(string directoryPath)
    {
        var filesDictionary = new Dictionary<string, byte[]>();

        var filePaths = Directory.GetFiles(directoryPath);

        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);

            var fileContent = File.ReadAllBytes(filePath);

            filesDictionary[fileName] = fileContent;
        }

        return filesDictionary;
    }
}