using Bogus;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Data;

public class Seed : ISeed
{
    private readonly DataContext _context;

    public Seed(DataContext context)
    {
        _context = context;
    }
    
    public async Task Seeding()
    {
        if (_context.Messages.Any()) return;
        
    }

    #region Fakeres
    
    #endregion
}