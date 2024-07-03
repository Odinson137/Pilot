using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class CompanyUserRepository(DataContext context, IMapper mapper) : BaseRepository<CompanyUser>(context, mapper), ICompanyUser
{

}