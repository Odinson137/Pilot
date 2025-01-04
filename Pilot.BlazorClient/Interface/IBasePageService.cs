using System.Linq.Expressions;
using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Interface;

public interface IBasePageService<TViewModel> where TViewModel : BaseViewModel
{
    Task<TViewModel> GetValueAsync(int valueId);

    Task<ICollection<TViewModel>> GetValuesAsync(int? skip = null, int? take = null, Expression<Func<TViewModel, bool>>? predicate = null);
    
    Task CreateValueAsync(TViewModel value);
    
    Task UpdateValueAsync(TViewModel value);
    
    Task DeleteValueAsync(int id);
}