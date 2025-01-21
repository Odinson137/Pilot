using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;

namespace Pilot.BlazorClient.Service.Pages;

public abstract class BasePageService<TViewModel>(IBaseModelService<TViewModel> baseModelService) : IBasePageService<TViewModel> where TViewModel : BaseViewModel
{
    public Task<TViewModel> GetValueAsync(int valueId)
    {
        return baseModelService.GetValueAsync(valueId);
    }

    public Task<ICollection<TViewModel>> GetValuesAsync(BaseFilter? baseFilter = null)
    {
        return baseModelService.GetValuesAsync(baseFilter);
    }

    public Task CreateValueAsync(TViewModel value)
    {
        return baseModelService.CreateValueAsync(value);
    }

    public Task UpdateValueAsync(TViewModel value)
    {
        return baseModelService.UpdateValueAsync(value);
    }

    public Task DeleteValueAsync(int id)
    {
        return baseModelService.DeleteValueAsync(id);
    }
}