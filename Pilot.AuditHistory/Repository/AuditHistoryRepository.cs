using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.AuditHistory.Interface;
using Pilot.Contracts.Base;

namespace Pilot.AuditHistory.Repository;

public class AuditHistoryRepository(DbContext context, IMapper mapper) : BaseRepository<Models.AuditHistory>(context, mapper), IAuditHistory
{
    
}