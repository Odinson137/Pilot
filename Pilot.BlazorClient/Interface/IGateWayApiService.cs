using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Interface;

public interface IGateWayApiService : IBaseHttpService
{
    Task<ICollection<TViewModel>> SendGetMessages<TOut, TViewModel>(
        string? url = null,
        BaseFilter? filter = null, 
        CancellationToken token = default)
        where TOut : BaseDto where TViewModel : BaseViewModel;

    Task<TViewModel> SendGetMessage<TOut, TViewModel>(
        int valueId,
        CancellationToken token = default)
        where TOut : BaseDto where TViewModel : BaseViewModel;
    
    public Task SendPostMessage<TMessage>(string? url, TMessage message, CancellationToken token) where TMessage : BaseDto;

    public Task SendPutMessage<TMessage>(string? url, TMessage message, CancellationToken token) where TMessage : BaseDto;
    
    public Task SendDeleteMessage<TMessage>(string? url, CancellationToken token) where TMessage : BaseDto;
}