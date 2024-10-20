using Pilot.Contracts.Data;

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
        if (_context.InfoMessages.Any()) return;
        
    }

    #region Fakeres
    
    #endregion
}