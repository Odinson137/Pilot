using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Storage.Data;
using Pilot.Storage.Interface;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Repository;

public class FileRepository(DataContext context, IMapper mapper) : BaseRepository<File>(context, mapper), IFileRepository;