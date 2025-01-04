using System.Linq.Expressions;
using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Service.Pages;

public class BaseModelService<TDto, TViewModel>(IGateWayApiService apiService, IMapper mapper) : IBasePageService<TViewModel>
    where TDto : BaseDto where TViewModel : BaseViewModel
{
    public async Task<TViewModel> GetValueAsync(int valueId)
    {
        return await apiService.SendGetMessage<TDto, TViewModel>(valueId);
    }

    public async Task<ICollection<TViewModel>> GetValuesAsync(int? skip = null, int? take = null, Expression<Func<TViewModel, bool>>? predicate = null)
    {
        var filter = new BaseFilter(skip, take);
        if (predicate is not null)
        {
            var expression = ((MethodCallExpression)predicate.Body).Arguments[1];
            var name = ((MemberExpression)((BinaryExpression)((Expression<Func<TViewModel, bool>>)expression).Body).Left).Member.Name;
            var value = int.Parse(((BinaryExpression)((Expression<Func<TViewModel, bool>>)expression).Body).Right.ToString());
            filter.WhereFilter = new ValueTuple<string, int>(name, value);
        }
        
        return await apiService.SendGetMessages<TDto, TViewModel>(filter: filter);
    }

    public async Task CreateValueAsync(TViewModel value)
    {
        await apiService.SendPostMessage(null, message: mapper.Map<TDto>(value));
    }

    public async Task UpdateValueAsync(TViewModel value)
    {
        await apiService.SendPutMessage(null, message: mapper.Map<TDto>(value));
    }

    public async Task DeleteValueAsync(int id)
    {
        await apiService.SendDeleteMessage<TDto>(id.ToString());
    }
}
