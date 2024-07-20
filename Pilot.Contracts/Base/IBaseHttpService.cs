namespace Pilot.Contracts.Base;

public interface IBaseHttpService
{
    public Task<TOut> SendGetMessages<TOut>(string url, BaseFilter? filter, CancellationToken token);
    
    public Task<TOut?> SendGetMessage<TOut>(string url, CancellationToken token);
    
    public Task SendPostMessage<TMessage>(string url, TMessage message, CancellationToken token);
    
    public Task SendPutMessage<TMessage>(string url, TMessage message, CancellationToken token);
    
    public Task SendDeleteMessage<TMessage>(string url, CancellationToken token);
}