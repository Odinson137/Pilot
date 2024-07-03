using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Repository;

public class FileRepository(DataContext context, IMapper mapper) : BaseRepository<File>(context, mapper), IFile
{

}