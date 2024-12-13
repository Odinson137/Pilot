using System.Collections;
using System.Reflection;
using MassTransit.Internals;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.Api.Services;

public class FileUrlService : IFileUrlService
{
    private readonly ILogger<FileUrlService> _logger;
    private readonly IModelService _modelService; 

    public FileUrlService(ILogger<FileUrlService> logger, IModelService modelService)
    {
        _logger = logger;
        _modelService = modelService;
    }

    public async Task GetUrlAsync<TResponse>(TResponse response)
    {
        var type = response!.GetType();

        var fileUrlsSet = new Dictionary<int, (PropertyInfo, object)>();

        var isList = false;
        if (type.IsGenericType)
        {
            isList = true;
            var enumerable = response as IEnumerable;
    
            // ReSharper disable once GenericEnumeratorNotDisposed
            var enumerator = enumerable?.GetEnumerator();
            if (enumerator != null && enumerator.MoveNext())
            {
                var first = enumerator.Current;
                type = first!.GetType();
            }
        }
        
        var hasFileType = typeof(IHasFile);
        switch (isList)
        {
            case false when type.HasInterface(hasFileType):
            {
                _logger.LogInformation($"Checked file {typeof(TResponse).Name}");

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(c => c.IsDefined(typeof(HasFileAttribute))).ToArray();

                FillFileProperties(properties, response, fileUrlsSet);
                break;
            }
            case true:
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(c => c.IsDefined(typeof(HasFileAttribute))).ToArray();

                if (type.HasInterface(hasFileType))
                {
                    _logger.LogInformation($"Checked collection of files {typeof(TResponse).Name}");
                    foreach (var listValue in (IEnumerable)response)
                    {
                        FillFileProperties(properties, listValue, fileUrlsSet);
                    }
                } else return;

                break;
            }
            default:
                return;
        }

        _logger.LogInformation("The Files in Models");
        _logger.LogClassInfo(fileUrlsSet);
        if (!fileUrlsSet.Any()) return;
        
        _logger.LogInformation("Exist some files");
        var filter = new BaseFilter(fileUrlsSet.Select(c => c.Key).ToArray());
        
        var files = await _modelService.GetValuesAsync<FileDto>(Urls.FileUrl, filter);
        foreach (var file in files)
        {
            var (property, currentValue) = fileUrlsSet[file.Id];
            var value = property.GetValue(currentValue);
            
            var fieldName = property.GetAttribute<HasFileAttribute>().Single().FieldName;

            var urlProperty = currentValue.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(c => c.Name == fieldName);
            
            if (value is IEnumerable)
            {
                var currentList = urlProperty.GetValue(currentValue);
                ((List<string>)currentList!).Add(file.Url!);
            }
            else
            {
                urlProperty.SetValue(currentValue, file.Url);
            }
            
        }
    }

    private static void FillFileProperties<TResponse>(PropertyInfo[] properties, TResponse response, Dictionary<int, (PropertyInfo, object)> fileUrls)
    {
        foreach (var property in properties)
        {
            var value = property.GetValue(response);
            
            if (value is ICollection<int> values)
            {
                if (!values.Any()) continue;
                
                foreach (var id in values)
                {
                    fileUrls.Add((int)id, (property, response)!);
                }
            }
            else
            {
                if (value == null) continue;

                fileUrls.Add((int)value!, (property, response)!);
            }
        }
    }
}