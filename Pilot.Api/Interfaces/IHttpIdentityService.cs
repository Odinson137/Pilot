using Pilot.Contracts.Base;

namespace Pilot.Api.Interfaces;

public interface IHttpIdentityService : IBaseHttpService
{
    public Task<TOut> SendPostMessage<TOut, TMessage>(string url, TMessage message, CancellationToken token);

    public Task SendPostMessage<TMessage>(string url, TMessage message, CancellationToken token);
}