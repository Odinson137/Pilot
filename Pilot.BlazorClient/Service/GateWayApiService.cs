using Pilot.BlazorClient.Interface;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;

namespace Pilot.BlazorClient.Service;

public class GateWayApiService(ILogger<BaseHttpService> logger, IHttpClientFactory httpClientFactory) : BaseHttpService(logger, httpClientFactory), IGateWayApiService
{
    public async Task SendPostMessage<TMessage>(string url, TMessage message, CancellationToken token)
    {
        var response = await HttpClient.PostAsJsonAsync(url, message, token);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync(token);
            throw new Exception(error);
        }
    }

    public async Task SendPutMessage<TMessage>(string url, TMessage message, CancellationToken token)
    {
        var response = await HttpClient.PostAsJsonAsync(url, message, token);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync(token);
            throw new Exception(error);
        }
    }

    public async Task SendDeleteMessage<TMessage>(string url, CancellationToken token)
    {
        var response = await HttpClient.DeleteAsync(url, token);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync(token);
            throw new Exception(error);
        }
    }
    
    protected override void HttpClientInit<TOut>()
    {
        HttpClient = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test"
            ? HttpSingleTone.Init.HttpClients["ApiServerUrl"]
            : httpClientFactory.CreateClient("ApiServerUrl");
    }
}