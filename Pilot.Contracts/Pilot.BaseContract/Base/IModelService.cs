namespace Pilot.Contracts.Base;

public interface IModelService
{
    public Task<TDto> GetValueByIdAsync<TDto>(int userId, CancellationToken token = default) where TDto : BaseDto;

    public Task<ICollection<TDto>> GetValuesAsync<TDto>(BaseFilter filter, CancellationToken token = default) where TDto : BaseDto;
}