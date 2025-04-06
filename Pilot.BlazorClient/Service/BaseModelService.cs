using System.Linq.Expressions;
using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;
using Serialize.Linq.Serializers;

namespace Pilot.BlazorClient.Service;

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
        var whereFilter = new WhereFilter();
        whereFilter.Init((predicate, value));
        filter.WhereFilter = whereFilter;

        return await apiService.SendGetMessages<TDto, TViewModel>(filter: filter);
    }

    public async Task<ICollection<TViewModel>> GetValuesAsync(ICollection<int> ids)
    {
        return await apiService.SendGetMessages<TDto, TViewModel>(filter: new BaseFilter(ids));
    }

    public async Task<ICollection<TViewModel>> GetValuesAsync(BaseFilter? filter)
    {
        return await apiService.SendGetMessages<TDto, TViewModel>(filter: filter);
    }
    
    public async Task<ICollection<TViewModel>> GetQueryValuesAsync(Expression<Func<TViewModel, object>> predicate)
    {
        var filter = new BaseFilter
        {
            SelectQuery = new ExpressionSerializer(new JsonSerializer()).SerializeText(predicate)
        };

        var client = await apiService.GetClientAsync<TDto>();
        
        var response = await client.PostAsJsonAsync("Query", filter);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        return await response.Content.ReadFromJsonAsync<ICollection<TViewModel>>() ?? [];
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

    public IGateWayApiService Client { get; } = apiService;
}