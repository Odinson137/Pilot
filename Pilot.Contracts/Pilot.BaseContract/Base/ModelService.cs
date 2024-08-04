using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Contracts.Base;

public abstract class ModelService<TDto> : BaseHttpService, IModelService<TDto> where TDto : BaseDto
{
    private readonly ILogger<ModelService<TDto>> _logger;
    private readonly IDistributedCache _cache;
    private readonly string _modelName = BaseExpendMethods.GetModelName<TDto>();
    
    public ModelService(ILogger<ModelService<TDto>> logger, IHttpClientFactory httpClientFactory, IDistributedCache cache, IConfiguration configuration, string clientName) 
        : base(logger, httpClientFactory, configuration, clientName)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<TDto> GetValueByIdAsync(int valueId, CancellationToken token)
    {
        _logger.LogInformation($"Getting {_modelName} by id - {valueId}");
        
        var cacheValue = await _cache.GetStringAsync($"{_modelName}-{valueId}", token);
        
        TDto? valueDto;
        if (string.IsNullOrEmpty(cacheValue))
        {
            _logger.LogInformation("Get value from cache");
            valueDto = await SendGetMessage<TDto>($"api/{_modelName}/{valueId}", default);
        }
        else
        {
            _logger.LogInformation("Get value from db");
            valueDto = JsonConvert.DeserializeObject<TDto>(cacheValue);
        }
        
        _logger.LogClassInfo(valueDto);
        return valueDto!;
    }

    public async Task<ICollection<TDto>> GetValuesAsync(BaseFilter? filter, CancellationToken token = default)
    {
        _logger.LogInformation($"Getting {_modelName} list");
        _logger.LogClassInfo(filter);
        
        var cacheValue = await _cache.GetStringAsync($"{_modelName}-{filter?.Skip}-{filter?.Take}", token);
        
        ICollection<TDto> valueDto;
        if (string.IsNullOrEmpty(cacheValue))
        {
            _logger.LogInformation("Get values from cache");
            valueDto = await SendGetMessages<List<TDto>>($"api/{_modelName}",  filter, default);
        }
        else
        {
            _logger.LogInformation("Get values from db");
            valueDto = JsonConvert.DeserializeObject<List<TDto>>(cacheValue)!;
        }
        
        _logger.LogClassInfo(valueDto);
        return valueDto;
    }
    
    // public async Task PostValue<T>(T message, CancellationToken token = default)
    // {
    //     _logger.LogInformation($"Getting {_modelName} to post");
    //     var response = SendPostMessage($"api/{_modelName}", message, token);
    // }
    //
    // public async Task SendPutMessage<T>(T message, CancellationToken token = default)
    // {
    //     _logger.LogInformation($"Getting {_modelName} to post");
    //     await SendPutMessage($"api/{_modelName}", message, token);
    // }
    //
    // public async Task SendDeleteMessage(string url, CancellationToken token = default)
    // {
    //     Logger.LogInformation($"Delete message to {url}");
    //     await HttpClient.DeleteAsync(url, cancellationToken: token);
    // }
}