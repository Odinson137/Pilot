﻿using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Pilot.Storage.Interface;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Service;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IStorageService _storageService;
    private readonly IFileRepository _fileRepository;
    private readonly IMapper _mapper;

    public FileService(IStorageService storageService, IFileRepository fileRepository, IMapper mapper,
        ILogger<FileService> logger)
    {
        _storageService = storageService;
        _fileRepository = fileRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> UploadFileAsync(FileDto fileDto)
    {
        _logger.LogInformation("Upload file");

        var fileModel = _mapper.Map<File>(fileDto);
        fileModel.Size = fileDto.ByteFormFile.GetSize();

        await _fileRepository.AddValueToContextAsync(fileModel);

        await _storageService.UploadFileAsync(fileDto);
        await _fileRepository.SaveAsync();

        return fileModel.Id;
    }

    public async Task ChangeFileAsync(FileDto fileDto)
    {
        _logger.LogInformation("Change file");

        var fileModel = fileDto.Id != 0
            ? await _fileRepository.GetRequiredByIdAsync(fileDto.Id)
            : throw new Exception("Поиск сущности с id = 0");

        var fileName = Guid.NewGuid().ToString();

        fileModel.Name = fileName;
        fileModel.Size = fileDto.ByteFormFile.GetSize();
        fileModel.ChangeAt = DateTime.Now;

        await _storageService.DeleteFileAsync(fileModel.Name);

        await _storageService.UploadFileAsync(fileDto);
        await _fileRepository.SaveAsync();
    }

    public async Task DeleteFileAsync(int id)
    {
        _logger.LogInformation($"Delete file with id {id}");

        var file = await _fileRepository.GetRequiredByIdAsync(id);
        await _storageService.DeleteFileAsync(file.Name);
        _fileRepository.Delete(file);
        await _fileRepository.SaveAsync();
    }

    public async Task<FileDto> GetFileAsync(int id)
    {
        _logger.LogInformation($"Get file with id {id}");

        var file = await _fileRepository.GetRequiredByIdAsync<FileDto>(id);
        file.ByteFormFile = await _storageService.GetFileAsync(file.Name);
        return file;
    }

    public async Task<FileDto> GetUrlAsync(string name)
    {
        _logger.LogInformation($"Get file url with id {name}");

        var file = await _fileRepository.GetValueUrlAsync(name);
        file.Url = _storageService.GetUrl(file.Name, file.Format);
        return file;
    }

    public async Task<ICollection<FileDto>> GetUrlsAsync(BaseFilter? filter)
    {
        _logger.LogInformation("Get file urls");

        var files = await _fileRepository.GetValuesAsync<FileDto>(filter);

        foreach (var file in files)
            file.Url = _storageService.GetUrl(file.Name, file.Format);

        return files;
    }
}