using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Exception.ApiExceptions;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Base;

public class BaseHttpService(
    ILogger<BaseHttpService> logger,
    IHttpClientFactory httpClientFactory)
    : IBaseHttpService
{
    protected readonly ILogger<BaseHttpService> Logger = logger;

    protected HttpClient? HttpClient;

    protected string? ClientName;
    
    protected static string GetFullUrl<TDto>(string? url) where TDto : BaseDto
    {
        return GetFullUrl<TDto>(url);
    }
    
    protected static string GetFullUrl<TDto>(string? url, BaseFilter? filter) where TDto : BaseDto
    {
        var stringBuilder = new StringBuilder($"api/{BaseExpendMethods.GetModelName<TDto>()}");

        if (url != null) stringBuilder.Append($"/{url}");
        
        // if (!queryParams.Any()) return stringBuilder.ToString();

        if (filter != null)
        {
            stringBuilder.Append("?");
            stringBuilder.Append($"filter={filter.ToJson()}");
        }
        
        return stringBuilder.ToString();
    }

    public async Task<ICollection<TOut>> SendGetMessages<TOut>(string? url = null, BaseFilter? filter = null, CancellationToken token = default) where TOut : BaseDto
    {
        Logger.LogInformation($"Send message to {typeof(TOut)}");

        var client = await GetClientAsync<TOut>();
        var response = await client.GetAsync(GetFullUrl<TOut>(url, filter), token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadFromJsonAsync<List<TOut>>(token);
        if (content == null) throw new BadRequestException($"Content from {typeof(TOut)} is null");

        return content;
    }

    public async Task<TOut> SendGetMessage<TOut>(string url, CancellationToken token = default) where TOut : BaseDto
    {
        Logger.LogInformation($"Send message by id = {url}");
        var client = await GetClientAsync<TOut>();
        var response = await client.GetAsync(GetFullUrl<TOut>(url, null), token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadFromJsonAsync<TOut>(token);
        if (content == null) throw new NotFoundException("Value not found");

        return content;
    }

    /// <summary>
    /// Инициализация конструктора для отправки данных в нужный сервис по DTO
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    protected virtual void HttpClientInit<TOut>()
    {
        var clientName = HttpNameService.GetHttpClientName(typeof(TOut));
        if (ClientName == clientName) return;
        
        HttpClient = CreateClient(clientName);
        ClientName = clientName;
    }

    protected HttpClient CreateClient(string clientName) => httpClientFactory.CreateClient(clientName);

    public virtual ValueTask<HttpClient> GetClientAsync<T>()
    {
        HttpClientInit<T>();
        return ValueTask.FromResult(HttpClient!);
    }
}