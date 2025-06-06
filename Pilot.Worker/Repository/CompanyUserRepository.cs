﻿using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Repository;

public class CompanyUserRepository(DataContext context, ReadOnlyDataContext readOnlyDataContext, IMapper mapper)
    : BaseReadWriteSplitRepository<CompanyUser>(context, readOnlyDataContext, mapper), ICompanyUser
{
}