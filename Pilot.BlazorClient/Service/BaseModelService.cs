using System.Linq.Expressions;
using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Service;

public class BaseModelService<TDto, TViewModel>(
    IGateWayApiService apiService,
    IMessengerService messengerService,
    IMapper mapper)
    : IBaseModelService<TViewModel>
    where TDto : BaseDto where TViewModel : BaseViewModel
{
    public async Task<TViewModel> GetValueAsync(int valueId)
    {
        return await apiService.SendGetMessage<TDto, TViewModel>(valueId);
    }

    public async Task<TViewModel?> GetValueAsync(
        params (Expression<Func<TViewModel, object>> predicate, object value)[] valueTuples)
    {
        return (await GetValuesAsync(valueTuples)).FirstOrDefault();
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

    public async Task<ICollection<TViewModel>> GetValuesAsync(
        params (Expression<Func<TViewModel, object>> predicate, object value)[] valueTuples)
    {
        var filter = new BaseFilter();
        var whereFilter = new WhereFilter();
        foreach (var valueTuple in valueTuples)
            whereFilter.Init(valueTuple);

        filter.WhereFilter = whereFilter;

        return await apiService.SendGetMessages<TDto, TViewModel>(filter: filter);
    }

    public async Task<ICollection<TViewModel>> GetValuesAsync(ICollection<int> ids)
    {
        return await apiService.SendGetMessages<TDto, TViewModel>(filter: new BaseFilter(ids));
    }

    public async Task<ICollection<TViewModel>> GetValuesAsync(BaseFilter filter)
    {
        return await apiService.SendGetMessages<TDto, TViewModel>(filter: filter);
    }

    public async Task CreateValueAsync(TViewModel value, Action<InfoMessageViewModel>? action = null)
    {
        try
        {
            if (action != null)
            {
                void Handler(InfoMessageViewModel message)
                {
                    action(message);
                    messengerService.OnActionNotification -= Handler;
                }

                messengerService.OnActionNotification += Handler;
            }

            await apiService.SendPostMessage(null, message: mapper.Map<TDto>(value));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update value {ex.Message}");
        }
    }

    public async Task UpdateValueAsync(TViewModel value, Action<InfoMessageViewModel>? action = null)
    {
        try
        {
            if (action != null)
            {
                void Handler(InfoMessageViewModel message)
                {
                    action(message);
                    messengerService.OnActionNotification -= Handler;
                }

                messengerService.OnActionNotification += Handler;
            }

            await apiService.SendPutMessage(null, message: mapper.Map<TDto>(value));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update value {ex.Message}");
        }
    }

    public async Task DeleteValueAsync(int id, Action<InfoMessageViewModel>? action = null)
    {
        try
        {
            if (action != null)
            {
                void Handler(InfoMessageViewModel message)
                {
                    action(message);
                    messengerService.OnActionNotification -= Handler;
                }

                messengerService.OnActionNotification += Handler;
            }

            await apiService.SendDeleteMessage<TDto>(id.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete value {ex.Message}");
        }
    }

    public IGateWayApiService Client { get; } = apiService;
}