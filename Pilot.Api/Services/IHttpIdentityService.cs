using Pilot.Contracts.Base;

namespace Pilot.Api.Services;

public interface IHttpIdentityService : IBaseHttpService
{
    public Task<TOut> SendPostMessage<TOut, TMessage>(string url, TMessage message, CancellationToken token);
}