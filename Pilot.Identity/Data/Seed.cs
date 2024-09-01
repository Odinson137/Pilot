using Bogus;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Contracts.Services;
using Pilot.Identity.Interfaces;
using Pilot.Identity.Models;

namespace Pilot.Identity.Data;

public class Seed : ISeed
{
    private readonly IPasswordCoder _passwordCoder;
    private readonly DataContext _context;
    private int _userId = 1;

    public Seed(IPasswordCoder passwordCoder, DataContext context)
    {
        _passwordCoder = passwordCoder;
        _context = context;
    }
    
    private Faker<User> GetUserFaker()
    {
        var fakeUser = new Faker<User>()
                .RuleFor(u => u.Id, (_, _) => _userId++)
                .RuleFor(u => u.Gender, (f, _) => f.Person.Gender)
                .RuleFor(u => u.Name, (f, u) => f.Name.FirstName(u.Gender))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.Name, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name, u.LastName))
                .RuleFor(u => u.Description, (f, _) => f.Lorem.Paragraphs().TakeOnly(1000))
                .RuleFor(u => u.Country, (f, _) => f.Address.Country())
                .RuleFor(u => u.City, (f, _) => f.Address.City())
                .RuleFor(u => u.AvatarImageId, (_, _) => _userId++) // fileId = userId - в сиде должно совпадать, можно оставить и так 
                .RuleFor(u => u.Password, f => _passwordCoder.GenerateSaltAndHashPassword(f.Internet.Password()).Item1)
                .RuleFor(u => u.Salt, (_, u) => _passwordCoder.GenerateSaltAndHashPassword(u.Password).Item2)
                .RuleFor(u => u.Birthday, (f, _) => f.Person.DateOfBirth);
        
        return fakeUser;
    }
    
    public async Task Seeding()
    {
        if (await _context.Users.AnyAsync()) return;
        
        var faker = GetUserFaker();
        var users = faker.Generate(Constants.SeedDataCount);
        await _context.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }
}