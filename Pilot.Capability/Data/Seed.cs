using Bogus;
using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Models;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;

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

        await AssignSkillsToUsersAsync(); // Связывание скилов с пользователями
        await AssignPostsToCompaniesAsync(posts); // Связывание должностей с компаниями

        await _context.SaveChangesAsync();

        await GenerateJobApplicationsAsync(posts); // Генерация заявок на вакансии

        await _context.SaveChangesAsync();
    }

    private List<Skill> _skills;

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
        "C#", "Java", "Python", "JavaScript", "HTML", "CSS", "Angular", "React",
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
            .RuleFor(u => u.Title, (_) => GetSkill())
            .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now));
    }

    private Faker<Post> GetPostFaker()
    {
        return new Faker<Post>()
            .RuleFor(u => u.Title, (_) => GetPostTitle())
            .RuleFor(u => u.Skills, (_) => GetRandomSkills())
            .RuleFor(u => u.Description, (f, _) => f.Lorem.Sentence(10))
            .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now));
    }

    private ICollection<Skill> GetRandomSkills()
    {
        return _skills.OrderBy(_ => Guid.NewGuid()).Take(3).ToList(); // Каждая должность будет иметь 3 случайных скила
    }

    private async Task AssignSkillsToUsersAsync()
    {
        for (var userId = 1; userId <= Constants.SeedDataCount; userId++)
        {
            var userSkills =
                _skills.OrderBy(_ => Guid.NewGuid()).Take(5).ToList(); // 5 случайных скила для каждого пользователя
            var userSkillEntities = userSkills.Select(skill => new UserSkill
            {
                UserId = userId,
                Skill = skill,
                ExperienceYears = new Random().Next(1, 10),
                SkillLevel = (SkillLevel)new Random().Next(0, 3)
            }).ToList();

            await _context.AddRangeAsync(userSkillEntities);
        }
    }

    private async Task AssignPostsToCompaniesAsync(ICollection<Post> posts)
    {
        var random = new Random();
        for (var companyId = 1; companyId <= 5; companyId++)
        {
            var randomPosts = posts.Skip(2 * (companyId - 1)).Take(2).ToList();

            foreach (var post in randomPosts)
            {
                var companyPost = GetCompanyPostFaker().Generate();
                companyPost.Post = post;
                companyPost.IsOpen = true;
                companyPost.ExpectedSalary = random.Next(5000, 10000);
                companyPost.RequiredExperienceYears = random.Next(1, 10);
                post.CompanyId = companyId;

                await _context.AddAsync(companyPost);
            }
        }
    }

    private Faker<CompanyPost> GetCompanyPostFaker()
    {
        return new Faker<CompanyPost>()
            .RuleFor(s => s.AdditionalRequirements, f => f.Lorem.Sentence(10));
    }

    private async Task GenerateJobApplicationsAsync(ICollection<Post> posts)
    {
        var faker = GetJobApplicationFaker();

        foreach (var post in posts)
        {
            var companyPosts = _context.CompanyPosts.Where(cp => cp.Post == post).ToList();
            foreach (var companyPost in companyPosts)
            {
                for (var i = 0; i < 3; i++) // Генерируем по 3 заявки на каждую вакансию
                {
                    var jobApplication = faker.Generate();
                    jobApplication.CompanyPost = companyPost;
                    jobApplication.UserId = new Random().Next(1, Constants.SeedDataCount);
                    jobApplication.Status = (ApplicationStatus)new Random().Next(0, 3);

                    await _context.AddAsync(jobApplication);
                }
            }
        }
    }

    private Faker<JobApplication> GetJobApplicationFaker()
    {
        return new Faker<JobApplication>()
            .RuleFor(a => a.Message, f => f.Lorem.Sentences(2));
    }
}