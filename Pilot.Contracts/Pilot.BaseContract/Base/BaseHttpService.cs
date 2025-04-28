using System.Net;
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
        return GetFullUrl<TDto>(url, null);
    }

    protected static string GetFullUrl<TDto>(string? url, BaseFilter? filter) where TDto : BaseDto
    {
        var stringBuilder = new StringBuilder($"api/{BaseExpendMethods.GetModelName<TDto>()}");

        if (!string.IsNullOrEmpty(url)) stringBuilder.Append($"/{url}");

        if (filter != null)
            stringBuilder.Append($"?filter={filter.ToJson()}");

        return stringBuilder.ToString();
    }

    public async Task<ICollection<TOut>> SendGetMessages<TOut>(string? url = null, BaseFilter filter = null,
        CancellationToken token = default) where TOut : BaseDto
    {
        Logger.LogInformation($"Send message to {typeof(TOut)}");

        var client = await GetClientAsync<TOut>();
        var requestUrl = GetFullUrl<TOut>(url, filter);
        Logger.LogInformation($"Full url: {requestUrl}");

        var response = await client.GetAsync(requestUrl, token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadFromJsonAsync<List<TOut>>(token);
        return content ?? [];
    }

    public async Task<TOut?> SendGetMessage<TOut>(string url, CancellationToken token = default) where TOut : BaseDto
    {
        Logger.LogInformation($"Send message by id = {url}");
        var client = await GetClientAsync<TOut>();
        var response = await client.GetAsync(GetFullUrl<TOut>(url, null), token);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            logger.LogError("Error: " + await response.Content.ReadAsStringAsync(token));
            return null;
        }

        var content = await response.Content.ReadFromJsonAsync<TOut>(token);

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