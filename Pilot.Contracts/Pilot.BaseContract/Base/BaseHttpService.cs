using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Data;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Base;

public class BaseHttpService(
    ILogger<BaseHttpService> logger,
    IHttpClientFactory httpClientFactory)
    : IBaseHttpService
{
    protected readonly ILogger<BaseHttpService> Logger = logger;

    private HttpClient? _httpClient;
    protected readonly IHttpClientFactory HttpClientFactory = httpClientFactory;

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == null)
                throw new NullReferenceException("Null http client");

            return _httpClient;
        }
        set => _httpClient = value;
    }

    protected static string GetFullUrl<TDto>(string? url, params (string, string)[] queryParams) where TDto : BaseDto
    {
        return GetFullUrl<TDto>(url, null, queryParams);
    }
    
    protected static string GetFullUrl<TDto>(string? url, BaseFilter? filter,  params (string, string)[] queryParams) where TDto : BaseDto
    {
        var stringBuilder = new StringBuilder($"api/{BaseExpendMethods.GetModelName<TDto>()}");

        if (url != null) stringBuilder.Append($"/{url}");
        
        // if (!queryParams.Any()) return stringBuilder.ToString();

        if (filter != null)
        {
            stringBuilder.Append("?");
            stringBuilder.Append($"filter={filter.ToJson()}");
        }
        
        // foreach (var param in queryParams)
        // {
        //     stringBuilder.Append($"{param.Item1}={param.Item2}&");
        // }
        //
        // stringBuilder.Remove(stringBuilder.Length-1, 1);
        
        return stringBuilder.ToString();
    }

    public async Task<ICollection<TOut>> SendGetMessages<TOut>(string? url = null, BaseFilter? filter = null, CancellationToken token = default, params (string, string)[] queryParams) where TOut : BaseDto
    {
        Logger.LogInformation($"Send message to {typeof(TOut)}");

        HttpClientInit<TOut>();

        var response = await HttpClient.GetAsync(GetFullUrl<TOut>(url, filter, queryParams), token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadFromJsonAsync<List<TOut>>(token);
        if (content == null) throw new BadRequestException($"Content from {typeof(TOut)} is null");

        return content;
    }

    public async Task<TOut> SendGetMessage<TOut>(string url, CancellationToken token = default, params (string, string)[] queryParams) where TOut : BaseDto
    {
        Logger.LogInformation($"Send message by id = {url}");

        HttpClientInit<TOut>();
        
        var response = await HttpClient.GetAsync(GetFullUrl<TOut>(url, null, queryParams), token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadFromJsonAsync<TOut>(token);
        if (content == null) throw new NotFoundException("User is not found");

        return content;
    }

    /// <summary>
    /// Инициализация конструктора для отправки данных в нужный сервис по DTO
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    protected virtual void HttpClientInit<TOut>()
    {
        if (_httpClient != null) return;

        var clientName = HttpNameService.GetHttpClientName(typeof(TOut));

        // Для тестов. По другому не придумал, как микросервисы дебажить, а Debug в тестах я люблю
        _httpClient = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test"
            ? HttpSingleTone.Init.HttpClients[clientName]
            : HttpClientFactory.CreateClient(clientName);
    }
}