using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Interface;

public interface IFileRepository : IBaseRepository<File>
{
    Task<ICollection<FileDto>> GetValuesUrlAsync(ICollection<string> names, CancellationToken token = default);

    Task<FileDto> GetValueUrlAsync(string name, CancellationToken token = default);
}