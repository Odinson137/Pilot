using System.Linq.Expressions;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Interface;

public interface IBaseModelService<TViewModel> where TViewModel : BaseViewModel
{
    Task<TViewModel> GetValueAsync(int valueId);

    Task<TViewModel?>
        GetValueAsync(params (Expression<Func<TViewModel, object>> predicate, object value)[] valueTuples);

    Task<ICollection<TViewModel>> GetValuesAsync(int? skip = null, int? take = null);

    Task<ICollection<TViewModel>> GetValuesAsync<T>(Expression<Func<TViewModel, T>> predicate, T value,
        int? skip = null, int? take = null) where T : IConvertible;

    Task<ICollection<TViewModel>> GetValuesAsync(
        params (Expression<Func<TViewModel, object>> predicate, object value)[] valueTuples);

    Task<ICollection<TViewModel>> GetValuesAsync(ICollection<int> ids);

    Task<ICollection<TViewModel>> GetValuesAsync(BaseFilter filter);

    Task CreateValueAsync(TViewModel value, Action<InfoMessageViewModel>? action = null);

    Task UpdateValueAsync(TViewModel value, Action<InfoMessageViewModel>? action = null);

    Task DeleteValueAsync(int id, Action<InfoMessageViewModel>? action = null);

    IGateWayApiService Client { get; }
}