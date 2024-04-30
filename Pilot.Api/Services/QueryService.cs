using Pilot.Api.Interfaces.Services;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Api.Services;

public class QueryService : IQueryService
{
    private readonly HttpClient _httpClient;

    public QueryService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ReceiverServer");
        
    }
    
    public async Task<TResponse> SendRequestAsync<TResponse>(string url, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(cancellationToken));   
        }
        
        var list = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        return list!;
    }
}