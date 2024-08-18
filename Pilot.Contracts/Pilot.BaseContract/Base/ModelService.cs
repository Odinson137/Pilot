using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Contracts.Base;

public class ModelService : BaseHttpService, IModelService
{
    private readonly IRedisService _redis;
    private readonly ILogger<ModelService> _logger;

    public ModelService(ILogger<ModelService> logger, IHttpClientFactory httpClientFactory,
        IRedisService redis, IConfiguration configuration)
        : base(logger, httpClientFactory, configuration)
    {
        _logger = logger;
        _redis = redis;
    }

    public virtual async Task<TDto> GetValueByIdAsync<TDto>(int valueId, CancellationToken token) where TDto : BaseDto
    {
        _logger.LogInformation($"Getting value by id - {valueId}");

        var modelName = BaseExpendMethods.GetModelName<TDto>();

        var cacheValue = await _redis.GetValueAsync($"{modelName}-{valueId}");

        TDto valueDto;
        if (string.IsNullOrEmpty(cacheValue))
        {
            _logger.LogInformation("Get value from cache");
            valueDto = await SendGetMessage<TDto>($"api/{modelName}/{valueId}", token);
        }
        else
        {
            _logger.LogInformation("Get value from db");
            valueDto = cacheValue.FromJson<TDto>();
        }

        _logger.LogClassInfo(valueDto);
        return valueDto;
    }

    public virtual async Task<ICollection<TDto>> GetValuesAsync<TDto>(BaseFilter? filter, CancellationToken token = default) where TDto : BaseDto
    {
        _logger.LogInformation($"Getting values list");
        _logger.LogClassInfo(filter);
        
        var modelName = BaseExpendMethods.GetModelName<TDto>();

        var cacheValue = await _redis.GetValueAsync($"{modelName}-{filter?.Skip}-{filter?.Take}");

        ICollection<TDto> valueDto;
        if (cacheValue.IsNull)
        {
            _logger.LogInformation("Get values from db");
            valueDto = await SendGetMessages<TDto>($"api/{modelName}", filter, default);
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