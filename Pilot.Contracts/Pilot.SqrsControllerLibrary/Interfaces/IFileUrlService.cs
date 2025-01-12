using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface IFileService
{
    public ValueTask GetUrlAsync<TResponse>(TResponse response);
    
    public ValueTask AddFileAsync<TResponse>(TResponse response, CancellationToken cancellationToken)  where TResponse : ICommand<BaseDto>;
}