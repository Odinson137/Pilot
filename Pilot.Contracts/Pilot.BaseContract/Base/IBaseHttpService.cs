namespace Pilot.Contracts.Base;

public interface IBaseHttpService
{
    public Task<ICollection<TOut>> SendGetMessages<TOut>(string? url = null, BaseFilter? filter = null,
        CancellationToken token = default, params (string, string)[] queryParams) where TOut : BaseDto;

    public Task<TOut> SendGetMessage<TOut>(string url, CancellationToken token = default,
        params (string, string)[] queryParams) where TOut : BaseDto;
}