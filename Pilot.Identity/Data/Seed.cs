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
    private Dictionary<string, byte[]> _imagesDictionary;

    public Seed(IPasswordCoder passwordCoder, DataContext context)
    {
        _passwordCoder = passwordCoder;
        _context = context;
    }
    
    private Faker<User> GetUserFaker()
    {
        var fakeUser = new Faker<User>()
                .RuleFor(u => u.Id, (f, u) => _userId++)
                .RuleFor(u => u.Gender, (f, u) => f.Person.Gender)
                .RuleFor(u => u.Name, (f, u) => f.Name.FirstName(u.Gender))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.Name, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name, u.LastName))
                .RuleFor(u => u.Description, (f, u) => f.Lorem.Paragraphs().TakeOnly(1000))
                .RuleFor(u => u.Country, (f, u) => f.Address.Country())
                .RuleFor(u => u.City, (f, u) => f.Address.City())
                .RuleFor(u => u.AvatarUrl, (f, u) => f.Internet.Avatar())
                .RuleFor(u => u.Password, f => _passwordCoder.GenerateSaltAndHashPassword(f.Internet.Password()).Item1)
                .RuleFor(u => u.Salt, (f, u) => _passwordCoder.GenerateSaltAndHashPassword(u.Password).Item2)
                .RuleFor(u => u.Birthday, (f, u) => f.Person.DateOfBirth);
        
        return fakeUser;
    }
    
    public Dictionary<string, byte[]> GetFilesFromDirectory(string directoryPath)
    {
        var filesDictionary = new Dictionary<string, byte[]>();

        var filePaths = Directory.GetFiles(directoryPath);

        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);

            var fileContent = File.ReadAllBytes(filePath);

            filesDictionary[fileName] = fileContent;
        }

        return filesDictionary;
    }
    
    public async Task Seeding()
    {
        if (await _context.Users.AnyAsync()) return;
        
        var faker = GetUserFaker();
        _imagesDictionary = GetFilesFromDirectory("SeedImages");
        var users = faker.Generate(_imagesDictionary.Count);
        await _context.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }
}