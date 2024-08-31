using Pilot.Contracts.Base;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Interface;

public interface IFileRepository : IBaseRepository<File>
{
    
}