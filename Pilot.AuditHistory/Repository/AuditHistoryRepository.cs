using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.AuditHistory.Data;
using Pilot.AuditHistory.Interface;
using Pilot.Contracts.Base;

namespace Pilot.AuditHistory.Repository;

public class AuditHistoryRepository(ClickHouseContext context, IMapper mapper) : BaseRepository<Models.AuditHistory>(context, mapper), IAuditHistory
{
    
}