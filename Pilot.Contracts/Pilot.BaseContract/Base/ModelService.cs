﻿using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Exception.ApiExceptions;
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

    public virtual async Task<TDto?> GetValueByUrlAsync<TDto>(string url, CancellationToken token = default)
        where TDto : BaseDto
    {
        _logger.LogInformation($"Getting value by url - {url}");

        var cacheKey = $"{BaseExpendMethods.GetModelName<TDto>()}-{url}";
        var cacheValue = await _redis.GetValueAsync(cacheKey);

        TDto? valueDto;
        if (string.IsNullOrEmpty(cacheValue))
        {
            _logger.LogInformation("Get value from db");
            valueDto = await SendGetMessage<TDto>(url, token);
            if (valueDto != null)
                await _redis.SetValueAsync(cacheKey, valueDto);
        }
        else
        {
            _logger.LogInformation("Get value from cache");
            valueDto = cacheValue.FromJson<TDto>();
        }

        _logger.LogClassInfo(valueDto);
        return valueDto;
    }

    public virtual Task<TDto?> GetValueByIdAsync<TDto>(int valueId, CancellationToken token = default)
        where TDto : BaseDto
    {
        return GetValueByUrlAsync<TDto>($"{valueId}", token);
    }

    public virtual async Task<ICollection<TDto>> GetValuesAsync<TDto>(string url, BaseFilter filter,
        CancellationToken token = default) where TDto : BaseDto
    {
        _logger.LogInformation("Getting values list");
        _logger.LogClassInfo(filter);

        // var cacheValue = await _redis.GetValuesAsync<TDto>(filter.GetKey<TDto>());
        //
        // ICollection<TDto> valueDto;
        // if (cacheValue == null)
        // {
        //     _logger.LogInformation("Get values from db");
        var valueDto = await SendGetMessages<TDto>(url, filter, token);
        //     await _redis.SetValuesAsync(filter.GetKey<TDto>(), valueDto);
        // }
        // else
        // {
        //     _logger.LogInformation("Get values from cache");
        //     valueDto = cacheValue.FromJson<List<TDto>>();
        // }

        _logger.LogClassInfo(valueDto);
        return valueDto;
    }

    public virtual Task<ICollection<TDto>> GetValuesAsync<TDto>(BaseFilter filter, CancellationToken token = default)
        where TDto : BaseDto
    {
        return GetValuesAsync<TDto>("", filter, token);
    }

    public virtual async Task<string> GetQueryValueAsync<TDto>(BaseFilter? filter, CancellationToken token = default)
        where TDto : BaseDto
    {
        Logger.LogInformation($"Send message to {typeof(TDto)}");

        var client = await GetClientAsync<TDto>();

        var response =
            await client.PostAsJsonAsync($"api/{BaseExpendMethods.GetModelName<TDto>()}/Query", filter, token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadAsStringAsync(token);
        return content;
    }
}