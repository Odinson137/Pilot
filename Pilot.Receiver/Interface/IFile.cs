using Pilot.Contracts.Base;
using File = Pilot.Receiver.Models.File;

namespace Pilot.Receiver.Interface;

public interface IFile : IBaseRepository<File>
{
}