namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface IFileUrlService
{
    public Task GetUrlAsync<TResponse>(TResponse response);
}