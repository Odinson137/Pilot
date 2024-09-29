namespace Pilot.Api.Interfaces;

public interface IFileUrlService
{
    public Task GetUrlAsync<TResponse>(TResponse response);
}