using System.Collections;
using System.Reflection;
using MassTransit.Internals;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Api.Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IModelService _modelService;
    private readonly IBaseMassTransitService _massTransitService;

    public FileService(ILogger<FileService> logger, IModelService modelService,
        IBaseMassTransitService massTransitService)
    {
        _logger = logger;
        _modelService = modelService;
        _massTransitService = massTransitService;
    }

    public async ValueTask GetUrlAsync<TResponse>(TResponse response)
    {
        var fileUrlsSet = new Dictionary<string, (PropertyInfo, object)>();

        var type = GetInternalValueType(response, out var isList);
        if (type is null) return;

        var hasFileType = typeof(IHasFile);
        switch (isList)
        {
            case false when type.HasInterface(hasFileType):
            {
                _logger.LogInformation($"Checked file {typeof(TResponse).Name}");

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(c => c.IsDefined(typeof(FileAttribute))).ToArray();

                FillFileProperties(properties, response, fileUrlsSet);
                break;
            }
            case true:
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(c => c.IsDefined(typeof(FileAttribute))).ToArray();

                if (type.HasInterface(hasFileType))
                {
                    _logger.LogInformation($"Checked collection of files {typeof(TResponse).Name}");
                    foreach (var listValue in (IEnumerable)response!)
                    {
                        FillFileProperties(properties, listValue, fileUrlsSet);
                    }
                }
                else return;

                break;
            }
            default:
                return;
        }

        _logger.LogInformation("The Files in Models");
        _logger.LogClassInfo(fileUrlsSet);
        if (!fileUrlsSet.Any()) return;

        _logger.LogInformation("Exist some files");
        // var filter = new BaseFilter(fileUrlsSet.Select(c => c.Key).ToJson(), FilterValueType.GetFileValue);
        //
        // var files = await _modelService.GetValuesAsync<FileDto>(Urls.FileUrl, filter);
        // foreach (var file in files)
        // {
        //     var (property, currentValue) = fileUrlsSet[file.Name];
        //     var value = property.GetValue(currentValue);
        //
        //     if (value is IEnumerable && property.PropertyType != typeof(string))
        //     {
        //         var list = (List<string>)property.GetValue(currentValue)!;
        //         list.Add(file.Url!);
        //     }
        //     else
        //         property.SetValue(currentValue, file.Url);
        // }
    }

    // эта реализация работает только если TResponse это не коллекция, в ином случае в системе пока и быть не может
    public async ValueTask ChangeFileAsync<TRequest>(TRequest response, CancellationToken cancellationToken)
        where TRequest : ICommand<BaseDto>
    {
        // var valueDto = response.ValueDto;
        // if (valueDto is IHasFile v && v.Files?.Count != 0) return;
        //
        // var type = GetInternalValueType(response, out _);
        // if (type is null) return;
        //
        // var value = (IHasFile)valueDto;
        //
        // foreach (var fileInfo in value.Files!)
        // {
        //     var key = fileInfo.Key;
        //     var bytes = fileInfo.Value;
        //
        //     var propertyInfo = type.GetProperties().Single(c => c.Name == key);
        //
        //     if (propertyInfo.IsDefined(typeof(IEnumerable)))
        //     {
        //         foreach (var fileByte in bytes)
        //         {
        //             var name = Guid.NewGuid().ToString();
        //             var file = new FileDto
        //             {
        //                 Name = name,
        //                 Type = string.Empty, // TODO придумать потом
        //                 Format = FileFormat.Image,
        //                 ByteFormFile = fileByte
        //             };
        //
        //             await _massTransitService.Publish(
        //                 new CreateCommandMessage<FileDto>(file, response.UserId), cancellationToken);
        //         }
        //
        //         propertyInfo.SetValue(value, null);
        //     }
        //     else
        //     {
        //         var name = Guid.NewGuid().ToString();
        //         var file = new FileDto
        //         {
        //             Name = name,
        //             Type = string.Empty, // TODO придумать потом
        //             Format = FileFormat.Image,
        //             ByteFormFile = bytes.First()
        //         };
        //
        //         propertyInfo.SetValue(value, name);
        //
        //         await _massTransitService.Publish(
        //             new CreateCommandMessage<FileDto>(file, 0), cancellationToken);
        //     }
        //
        //     propertyInfo.SetValue(value, null);
        // }
    }

    private static Type? GetInternalValueType<TResponse>(TResponse response, out bool isList)
    {
        var type = response!.GetType();
        isList = response is IEnumerable;
        if (!isList) return type;

        type = ((IEnumerable<object>)response).FirstOrDefault()?.GetType();
        return type;
    }

    private static void FillFileProperties<TResponse>(PropertyInfo[] properties, TResponse response,
        Dictionary<string, (PropertyInfo, object)> fileUrls)
    {
        foreach (var property in properties)
        {
            var value = property.GetValue(response);

            if (value is ICollection<string> values)
            {
                if (!values.Any()) continue;

                foreach (var name in values)
                {
                    fileUrls.Add(name, (property, response)!);
                }
                
                ((List<string>)value).Clear();
            }
            else
            {
                if (value == null) continue;

                fileUrls.Add((string)value, (property, response)!);
            }
        }
    }
}