using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface IFileService
{
    public ValueTask GetUrlAsync<TResponse>(TResponse response);

    public ValueTask ChangeFileAsync<TRequest>(TRequest response, CancellationToken cancellationToken)
        where TRequest : ICommand<BaseDto>;
}