﻿using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;
using Pilot.Worker.Models;

namespace Pilot.Worker.Repository;

public class TeamRepository(DataContext context, IMapper mapper) : BaseRepository<Team>(context, mapper), ITeam
{
}