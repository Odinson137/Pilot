using System.Linq.Expressions;
using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IBaseModelService<TViewModel> where TViewModel : BaseViewModel
{
    Task<TViewModel> GetValueAsync(int valueId);

    Task<ICollection<TViewModel>> GetValuesAsync(int? skip = null, int? take = null);

    Task<ICollection<TViewModel>> GetValuesAsync<T>(Expression<Func<TViewModel, T>> predicate, T value,
        int? skip = null, int? take = null) where T : IConvertible;

    Task<ICollection<TViewModel>> GetValuesAsync(params int[] ints);
    
    Task CreateValueAsync(TViewModel value);
    
    Task UpdateValueAsync(TViewModel value);
    
    Task DeleteValueAsync(int id);
}