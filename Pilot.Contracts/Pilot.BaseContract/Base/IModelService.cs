
namespace Pilot.Contracts.Base;

public interface IModelService<TDto>
{
    public Task<TDto> GetValueByIdAsync(int userId, CancellationToken token = default);

    public Task<ICollection<TDto>> GetValuesAsync(BaseFilter? filter, CancellationToken token = default);
}