using Bogus;
using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Models;
using Pilot.Contracts.Data;

namespace Pilot.Capability.Data;

public class Seed : ISeed
{
    private readonly DataContext _context;

    public Seed(DataContext context)
    {
        _context = context;
    }

    public async Task Seeding()
    {
        if (await _context.Posts.AnyAsync()) return;

        _skills = GetSkillFaker().Generate(30); // Генерация 30 скилов
        await _context.AddRangeAsync(_skills);

        var posts = GetPostFaker().Generate(10); // Генерация 10 должностей
        await _context.AddRangeAsync(posts);

        await AssignSkillsToUsersAsync(_skills); // Связывание скилов с пользователями
        await AssignPostsToCompaniesAsync(posts); // Связывание должностей с компаниями

        await _context.SaveChangesAsync();
    }

    private ICollection<Skill> _skills; 

    private readonly List<string> _examplePosts =
    [
        "Frontend programmer",
        "Backend programmer",
        "Fullstack developer",
        "Data Scientist",
        "Project Manager",
        "System Architect",
        "QA Engineer",
        "DevOps Engineer",
        "UI/UX Designer",
        "Technical Writer"
    ];

    private readonly List<string> _exampleSkills =
    [
        "C#", "Java", "Python", "JavaScript", "HTML", "CSS", "Angular", "React", "Vue",
        "SQL", "NoSQL", "Entity Framework", "Django", "Flask", "Ruby", "PHP", "Docker",
        "Kubernetes", "AWS", "Azure", "GCP", "Git", "CI/CD", "Agile", "Scrum", "TDD",
        "Machine Learning", "AI", "Big Data", "GraphQL", "REST API"
    ];

    private string GetSkill()
    {
        var random = new Random();
        var index = random.Next(0, _exampleSkills.Count);
        var value = _exampleSkills[index];
        _exampleSkills.Remove(value);
        return value;
    }
    
    private string GetPostTitle()
    {
        var random = new Random();
        var index = random.Next(0, _examplePosts.Count);
        var value = _examplePosts[index];
        _examplePosts.Remove(value);
        return value;
    }

    private Faker<Skill> GetSkillFaker()
    {
        return new Faker<Skill>()
            .RuleFor(u => u.Title, (_, _) => GetSkill())
            .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now));
    }

    private Faker<Post> GetPostFaker()
    {
        return new Faker<Post>()
            .RuleFor(u => u.Title, f => GetPostTitle())
            .RuleFor(u => u.Skills, f => GetRandomSkills())
            .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now));
    }

    private ICollection<Skill> GetRandomSkills()
    {
        return new Faker<Skill>()
            .RuleFor(s => s.Title, f => f.PickRandom(_exampleSkills))
            .Generate(3); // Каждая должность будет иметь 3 случайных скила
    }

    private async Task AssignSkillsToUsersAsync(ICollection<Skill> skills)
    {
        for (int userId = 1; userId <= 30; userId++)
        {
            var userSkills = skills.OrderBy(_ => Guid.NewGuid()).Take(3).ToList(); // 3 случайных скила для каждого пользователя
            var userSkillEntities = userSkills.Select(skill => new UserSkill
            {
                UserId = userId,
                Skill = skill,
                ExperienceYears = new Random().Next(1, 10),
            }).ToList();

            await _context.AddRangeAsync(userSkillEntities);
        }
    }

    private async Task AssignPostsToCompaniesAsync(ICollection<Post> posts)
    {
        for (int companyId = 1; companyId <= 5; companyId++)
        {
            var companyPosts = posts.OrderBy(_ => Guid.NewGuid()).Take(2).ToList(); // 2 должности для каждой компании
            var companyPostEntities = companyPosts.Select(post => new CompanyPost
            {
                CompanyId = companyId,
                Post = post,
                Skills = post.Skills
            }).ToList();

            await _context.AddRangeAsync(companyPostEntities);
        }
    }
}