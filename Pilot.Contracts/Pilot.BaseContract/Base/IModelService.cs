namespace Pilot.Contracts.Base;

public interface IModelService
{
    public Task<TDto> GetValueByIdAsync<TDto>(string url, CancellationToken token = default) where TDto : BaseDto;
    
    public Task<TDto> GetValueByIdAsync<TDto>(int valueId, CancellationToken token = default) where TDto : BaseDto;

    public Task<ICollection<TDto>> GetValuesAsync<TDto>(string url, BaseFilter? filter, CancellationToken token = default)
        where TDto : BaseDto;
    
    public Task<ICollection<TDto>> GetValuesAsync<TDto>(BaseFilter? filter, CancellationToken token = default) where TDto : BaseDto;
}