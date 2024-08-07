﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Repository;

public class CompanyRepository(DataContext context, IMapper mapper) : BaseRepository<Company>(context, mapper), ICompany
{
    public async Task<bool> CheckCompanyTitleExistAsync(string title)
    {
        return await DbSet.AnyAsync(c => c.Title == title);
    }
}