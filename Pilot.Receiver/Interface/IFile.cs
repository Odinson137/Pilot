using Pilot.Contracts.Base;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Interface;

public interface IFile : IBaseRepository<File>
{
}