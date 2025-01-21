using System.Linq.Expressions;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Interface;

public interface IBaseModelService<TViewModel> where TViewModel : BaseViewModel
{
    Task<TViewModel> GetValueAsync(int valueId);

    Task<ICollection<TViewModel>> GetValuesAsync(int? skip = null, int? take = null);

    Task<ICollection<TViewModel>> GetValuesAsync<T>(Expression<Func<TViewModel, T>> predicate, T value,
        int? skip = null, int? take = null) where T : IConvertible;

    Task<ICollection<TViewModel>> GetValuesAsync(ICollection<int> ids);

    Task<ICollection<TViewModel>> GetValuesAsync(BaseFilter? filter);
    
    Task CreateValueAsync(TViewModel value);
    
    Task UpdateValueAsync(TViewModel value);
    
    Task DeleteValueAsync(int id);

    IGateWayApiService Client { get; }
}