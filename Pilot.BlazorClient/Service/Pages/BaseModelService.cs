using System.Linq.Expressions;
using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Service.Pages;

public class BaseModelService<TDto, TViewModel>(IGateWayApiService apiService, IMapper mapper) : IBaseModelService<TViewModel>
    where TDto : BaseDto where TViewModel : BaseViewModel
{
    public async Task<TViewModel> GetValueAsync(int valueId)
    {
        return await apiService.SendGetMessage<TDto, TViewModel>(valueId);
    }

    // пока добавил поддержку только для int. Можно легко сделать для T, если надо будет
    public async Task<ICollection<TViewModel>> GetValuesAsync(int? skip = null, int? take = null, Expression<Func<TViewModel, int>>? predicate = null, int? value = default)
    {
        var filter = new BaseFilter(skip, take);
        if (predicate is not null)
        {
            var fieldsName = predicate.Body.ToString().Split(".").Skip(1).ToList();
            if (value is null) throw new ArgumentNullException("Ты чёт попутал");
            
            var names = string.Join(".", fieldsName);
            filter.WhereFilter = new ValueTuple<string, int>(names, value.Value);
        }
        
        return await apiService.SendGetMessages<TDto, TViewModel>(filter: filter);
    }

    public async Task<ICollection<TViewModel>> GetValuesAsync(params int[] ints)
    {
        return await apiService.SendGetMessages<TDto, TViewModel>(filter: new BaseFilter(ints));
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
