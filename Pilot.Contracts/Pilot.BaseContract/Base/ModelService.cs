using Microsoft.Extensions.Logging;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Contracts.Base;

public class ModelService : BaseHttpService, IModelService
{
    private readonly IRedisService _redis;
    private readonly ILogger<ModelService> _logger;

    public ModelService(
        ILogger<ModelService> logger, 
        IHttpClientFactory httpClientFactory,
        IRedisService redis)
        : base(logger, httpClientFactory)
    {
        _logger = logger;
        _redis = redis;
    }

    public virtual async Task<TDto> GetValueByIdAsync<TDto>(int valueId, CancellationToken token = default) where TDto : BaseDto
    {
        _logger.LogInformation($"Getting value by id - {valueId}");

        var cacheValue = await _redis.GetValueAsync($"{BaseExpendMethods.GetModelName<TDto>()}-{valueId}");

        TDto valueDto;
        if (string.IsNullOrEmpty(cacheValue))
        {
            _logger.LogInformation("Get value from cache");
            valueDto = await SendGetMessage<TDto>(valueId, token);
        }
        else
        {
            _logger.LogInformation("Get value from db");
            valueDto = cacheValue.FromJson<TDto>();
        }

        _logger.LogClassInfo(valueDto);
        return valueDto;
    }

    public virtual async Task<ICollection<TDto>> GetValuesAsync<TDto>(BaseFilter filter, CancellationToken token = default) where TDto : BaseDto
    {
        _logger.LogInformation($"Getting values list");
        _logger.LogClassInfo(filter);
        
        var modelName = BaseExpendMethods.GetModelName<TDto>();

        var cacheValue = await _redis.GetValuesAsync<TDto>(filter.Key);

        ICollection<TDto> valueDto;
        if (cacheValue == null)
        {
            _logger.LogInformation("Get values from db");
            valueDto = await SendGetMessages<TDto>(null, token, [("value", $"{filter}")]);
        }
        else
        {
            _logger.LogInformation("Get values from cache");
            valueDto = cacheValue.FromJson<List<TDto>>();
        }

        _logger.LogClassInfo(valueDto);
        return valueDto;
    }
}