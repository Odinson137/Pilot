using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Interface;

public interface IBasePageService<TViewModel> where TViewModel : BaseViewModel
{
    Task<TViewModel> GetValueAsync(int valueId);

    Task<ICollection<TViewModel>> GetValuesAsync(BaseFilter baseFilter = null);
    
    Task CreateValueAsync(TViewModel value, Action<InfoMessageViewModel>? action = null);
    
    Task UpdateValueAsync(TViewModel value, Action<InfoMessageViewModel>? action = null);
    
    Task DeleteValueAsync(int id, Action<InfoMessageViewModel>? action = null);
}