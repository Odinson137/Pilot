namespace Pilot.Contracts.Base;

public interface IBaseHttpService
{
    public Task<ICollection<TOut>> SendGetMessages<TOut>(string? url = null, CancellationToken token = default, params (string, string)[] queryParams) where TOut : BaseDto;

    public Task<TOut> SendGetMessage<TOut>(int valueId, CancellationToken token = default, params (string, string)[] queryParams) where TOut : BaseDto;
}