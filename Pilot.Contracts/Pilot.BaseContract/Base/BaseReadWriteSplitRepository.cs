using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public abstract class BaseReadWriteSplitRepository<T>(DbContext writeContext, DbContext readContext, IMapper mapper)
    : BaseRepository<T>(readContext, mapper)
    where T : BaseModel
{
    public override DbContext Context { get; } = writeContext;
}