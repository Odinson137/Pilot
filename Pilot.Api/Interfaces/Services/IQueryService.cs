namespace Pilot.Api.Interfaces.Services;

public interface IQueryService
{
    public Task<TResponse> SendRequestAsync<TResponse>(string url, CancellationToken cancellationToken);
}