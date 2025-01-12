using Pilot.Contracts.Base;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Interface;

public interface IFileRepository : IBaseRepository<File>
{
    Task<ICollection<TOut>> GetValuesUrlAsync<TOut>(ICollection<string> names, CancellationToken token = default)
        where TOut : BaseDto;
}