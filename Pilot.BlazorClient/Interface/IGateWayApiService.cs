using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Interface;

public interface IGateWayApiService : IBaseHttpService
{
    public Task SendPostMessage<TMessage>(string url, TMessage message, CancellationToken token);

    public Task SendPutMessage<TMessage>(string url, TMessage message, CancellationToken token);
    
    public Task SendDeleteMessage<TMessage>(string url, CancellationToken token);
}