using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Storage.Interface;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Repository;

public class FileRepository(DbContext context, IMapper mapper) : BaseRepository<File>(context, mapper), IFileRepository
{
    
}