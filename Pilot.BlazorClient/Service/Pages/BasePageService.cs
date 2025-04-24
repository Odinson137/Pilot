using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Service.Pages;

public abstract class BasePageService<TViewModel>(
    IBaseModelService<TViewModel> baseModelService,
    IMessengerService messengerService) : IBasePageService<TViewModel> where TViewModel : BaseViewModel
{
    public Task<TViewModel> GetValueAsync(int valueId)
    {
        return baseModelService.GetValueAsync(valueId);
    }

    public Task<ICollection<TViewModel>> GetValuesAsync(BaseFilter baseFilter = null)
    {
        return baseModelService.GetValuesAsync(baseFilter);
    }

    public async Task CreateValueAsync(TViewModel value, Action<string>? action = null)
    {
        try
        {
            if (action != null)
            {
                void Handler(string message)
                {
                    action(message);
                    messengerService.OnActionNotification -= Handler;
                }
            
                messengerService.OnActionNotification += Handler;
            }
            await baseModelService.CreateValueAsync(value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create value {ex.Message}");
        }
    }

    public async Task UpdateValueAsync(TViewModel value, Action<string>? action = null)
    {
        try
        {
            if (action != null)
            {
                void Handler(string message)
                {
                    action(message);
                    messengerService.OnActionNotification -= Handler;
                }
            
                messengerService.OnActionNotification += Handler;
            }
            await baseModelService.UpdateValueAsync(value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update value {ex.Message}");
        }
    }

    public async Task DeleteValueAsync(int id, Action<string>? action = null)
    {
        try
        {
            if (action != null)
            {
                void Handler(string message)
                {
                    action(message);
                    messengerService.OnActionNotification -= Handler;
                }
            
                messengerService.OnActionNotification += Handler;
            }
            await baseModelService.DeleteValueAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete value {ex.Message}");
        }
    }
}