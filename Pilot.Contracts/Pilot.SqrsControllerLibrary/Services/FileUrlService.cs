using System.Collections;
using System.Reflection;
using MassTransit.Internals;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Services;

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

        var awaits = new List<Task>();
        
        var hasFileType = typeof(IHasFile);
        if (type.HasInterface(hasFileType))
        {
            _logger.LogInformation($"Checked file {typeof(TResponse).Name}");

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.IsDefined(typeof(HasFileAttribute))) continue;
                var value = property.GetValue(response);

                // TODO придумать что нибудь получше, чтоб получать сразу охапку данных
                Task task;
                if (value is ICollection<BaseDto> values)
                {
                    var ids = values.Select(c => c.Id).ToArray();
                    var filter = new BaseFilter(ids);
                    task = _modelService.GetValuesAsync<FileDto>("Url", filter);
                }
                else
                {
                    task = _modelService.GetValueByIdAsync<FileDto>($"Url/{(int)value!}");
                }
                
                awaits.Add(task);
            }
        } else if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type))
        {
            _logger.LogInformation($"Checked collection of files {typeof(TResponse).Name}");

        }

        await Task.WhenAll(awaits);
    }
}