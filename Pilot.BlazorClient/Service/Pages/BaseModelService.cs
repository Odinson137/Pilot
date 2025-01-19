using System.Linq.Expressions;
using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Service.Pages;

public class BaseModelService<TDto, TViewModel>(IGateWayApiService apiService, IMapper mapper)
    : IBaseModelService<TViewModel>
    where TDto : BaseDto where TViewModel : BaseViewModel
{
    public async Task<TViewModel> GetValueAsync(int valueId)
    {
        return await apiService.SendGetMessage<TDto, TViewModel>(valueId);
    }

    public async Task<ICollection<TViewModel>> GetValuesAsync(int? skip = null, int? take = null)
    {
        var filter = new BaseFilter(skip, take);
        return await apiService.SendGetMessages<TDto, TViewModel>(filter: filter);
    }

    public async Task<ICollection<TViewModel>> GetValuesAsync<T>(Expression<Func<TViewModel, T>> predicate, T value,
        int? skip = null, int? take = null) where T : IConvertible
    {
        var filter = new BaseFilter(skip, take);
        var fieldsName = predicate.Body.ToString().Split(".").Skip(1).ToList();
        if (value is null) throw new ArgumentNullException("Ты чёт попутал");

        var names = string.Join(".", fieldsName);
        filter.WhereFilter = new WhereFilter((names, value));

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