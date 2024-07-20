using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Contracts.Base;

public abstract class ModelService<TDto> : BaseHttpService, IModelService<TDto> where TDto : BaseDto
{
    private readonly ILogger<ModelService<TDto>> _logger;
    private readonly IDistributedCache _cache;
    
    public ModelService(ILogger<ModelService<TDto>> logger, IHttpClientFactory httpClientFactory, IDistributedCache cache, string clientName) 
        : base(logger, httpClientFactory, clientName)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<TDto> GetValueByIdAsync(int valueId, CancellationToken token)
    {
        var modelName = BaseExpendMethods.GetModelName<TDto>();
        _logger.LogInformation($"Getting {modelName} by id - {valueId}");
        
        var cacheValue = await _cache.GetStringAsync($"{modelName}-{valueId}", token);
        
        TDto? valueDto;
        if (string.IsNullOrEmpty(cacheValue))
        {
            _logger.LogInformation("Get value from cache");
            valueDto = await SendGetMessage<TDto>($"api/{modelName}/{valueId}", default);
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
        var modelName = BaseExpendMethods.GetModelName<TDto>();
        _logger.LogInformation($"Getting {modelName} list");
        _logger.LogClassInfo(filter);
        
        var cacheValue = await _cache.GetStringAsync($"{modelName}-{filter?.Skip}-{filter?.Take}", token);
        
        ICollection<TDto> valueDto;
        if (string.IsNullOrEmpty(cacheValue))
        {
            _logger.LogInformation("Get values from cache");
            valueDto = await SendGetMessages<List<TDto>>($"api/{modelName}",  filter, default);
        }
        else
        {
            _logger.LogInformation("Get values from db");
            valueDto = JsonConvert.DeserializeObject<List<TDto>>(cacheValue)!;
        }
        
        _logger.LogClassInfo(valueDto);
        return valueDto;
    }
    
    // public async Task PostValue(T message, CancellationToken token = default)
    // {
    //     var modelName = BaseExpendMethods.GetModelName<T>();
    //     _logger.LogInformation($"Getting {modelName} to post");
    //     var response = SendPostMessage($"api/{modelName}", message, token);
    // }
    //
    // public async Task SendPutMessage(T message, CancellationToken token = default)
    // {
    //     var modelName = BaseExpendMethods.GetModelName<T>();
    //     _logger.LogInformation($"Getting {modelName} to post");
    //     await SendPutMessage($"api/{modelName}", message, token);
    // }
    //
    // public async Task SendDeleteMessage(string url, CancellationToken token = default)
    // {
    //     Logger.LogInformation($"Delete message to {url}");
    //     var response = await HttpClient.DeleteAsync(url, cancellationToken: token);
    //     if (!response.IsSuccessStatusCode)
    //     {
    //         throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
    //     }
    // }
}