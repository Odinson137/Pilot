using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Data;

public class Seed : ISeed
{
    private readonly DataContext _context;

    private List<string> CompanyRoles =
    [
        "Owner", "Developer", "Designer"
    ];

    public Seed(DataContext context)
    {
        _context = context;
    }
    
    public async Task Seeding()
    {
        if (await _context.CompanyUsers.AnyAsync()) return;
    }
    
    // private Faker<CompanyRole> GetFileDtoFaker()
    // {
    //     var fakeUser = new Faker<CompanyRole>()
    //             .RuleFor(u => u.Title, (f, _u) => f.)
    //         ;
    //     
    //     return fakeUser;
    // }
}