using Bogus;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;
using Pilot.Worker.Models;

namespace Pilot.Worker.Data;

public class Seed : ISeed
{
    private readonly DataContext _context;

    private readonly List<CompanyRole> _companyRoles =
    [
        new() { Title = "Owner", IsBaseRole = true },
        new() { Title = "Developer", IsBaseRole = true },
        new() { Title = "Designer", IsBaseRole = true }
    ];

    public Seed(DataContext context)
    {
        _context = context;
    }

    public async Task Seeding()
    {
        if (_context.Companies.Any()) return;

        var transaction = await _context.Database.BeginTransactionAsync();

        // Добавляем базовые роли
        await _context.CompanyRoles.AddRangeAsync(_companyRoles);
        await _context.SaveChangesAsync();

        var companyFaker = GetCompanyFaker();
        var companyRoleFaker = GetCompanyRoleFaker();
        var companyUserFaker = GetCompanyUserFaker();
        var projectFaker = GetProjectFaker();
        var projectTaskFaker = GetProjectTaskFaker();
        var teamFaker = GetTeamFaker();
        var taskInfoFaker = GetTaskInfoFaker();
        var rand = new Random(); // Для taskInfo.TimeSpent

        // Счетчики для детерминированных индексов
        var postId = 1;
        var companyIndex = 0;
        var roleIndex = 0;
        var userIndex = 0;
        var projectIndex = 0;
        var teamIndex = 0;
        var taskIndex = 0;

        // Генерация 5 компаний
        for (var i = 0; i < 5; i++, postId++, companyIndex++)
        {
            var company = companyFaker.Generate();
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            // Генерация 5 ролей компании
            company.CompanyRoles = companyRoleFaker.Generate(5);
            foreach (var role in company.CompanyRoles)
            {
                _context.CompanyRoles.Add(role);
            }
            await _context.SaveChangesAsync();

            var roles = await _context.CompanyRoles
                .Where(c => c.Companies.Any(x => x.Id == company.Id) || c.IsBaseRole)
                .ToListAsync();

            // Главный человек (Owner)
            var ownerCompany = new CompanyUser
            {
                Company = company,
                CompanyRole = roles.First(c => c.Title == "Owner")
            };
            await _context.CompanyUsers.AddAsync(ownerCompany);

            // Генерация 5 пользователей
            var companyUsers = companyUserFaker.Generate(5);
            for (var j = 0; j < companyUsers.Count; j++)
            {
                var user = companyUsers[j];
                user.Company = company;
                user.PostId = postId + (j % 2); // Детерминированное значение
                user.CompanyRole = roles[roleIndex % roles.Count];
                roleIndex++;
                await _context.CompanyUsers.AddAsync(user);
            }
            await _context.SaveChangesAsync();

            // Генерация 5 проектов
            var projects = projectFaker.Generate(5);
            for (var j = 0; j < projects.Count; j++)
            {
                var project = projects[j];
                project.Company = company;
                project.CreatedBy = ownerCompany;
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
            }

            var users = await _context.CompanyUsers
                .Where(c => c.Company.Id == company.Id)
                .ToListAsync();

            var projectsDb = await _context.Projects
                .Where(c => c.Company.Id == company.Id)
                .ToListAsync();

            // Генерация 6 команд
            var teams = teamFaker.Generate(6);
            for (var j = 0; j < teams.Count; j++)
            {
                var team = teams[j];
                team.Project = projectsDb[teamIndex % projectsDb.Count];
                teamIndex++;

                // Добавляем 2 пользователей в команду
                for (var a = 0; a < 2; a++)
                {
                    var user = users[(userIndex + a) % users.Count];
                    if (!team.CompanyUsers.Contains(user))
                    {
                        team.CompanyUsers.Add(user);
                    }
                }
                userIndex++;
                _context.Teams.Add(team);

                // Генерация 15 задач
                var projectTasks = projectTaskFaker.Generate(15);
                for (var k = 0; k < projectTasks.Count; k++)
                {
                    var task = projectTasks[k];
                    task.CompanyUser = users[taskIndex % users.Count];
                    task.CreatedBy = users[(taskIndex + 1) % users.Count];
                    task.Team = team;
                    _context.ProjectTasks.Add(task);
                    taskIndex++;
                }
                await _context.SaveChangesAsync();
            }

            var projectTasksDb = await _context.ProjectTasks
                .Where(c => c.Team.Project.Company.Id == company.Id)
                .ToListAsync();

            // Генерация 2 TaskInfo для каждой задачи
            foreach (var projectTask in projectTasksDb)
            {
                var taskInfos = taskInfoFaker.Generate(2);
                foreach (var taskInfo in taskInfos)
                {
                    taskInfo.CreatedBy = ownerCompany;
                    taskInfo.ProjectTask = projectTask;
                    taskInfo.TimeSpent = new TimeSpan(rand.Next(1, 10), rand.Next(0, 59), rand.Next(0, 59)); // Сохраняем случайность
                    _context.TaskInfos.Add(taskInfo);
                }
            }
            await _context.SaveChangesAsync();
        }

        await transaction.CommitAsync();
    }

    #region Fakeres

    private Faker<CompanyRole> GetCompanyRoleFaker()
    {
        var fake = new Faker<CompanyRole>()
                .RuleFor(u => u.Title, (f, _) => f.Lorem.Word())
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
            ;

        return fake;
    }

    private int _logoId = 31; // смотреть в сиде в проекте Storage. Там доступно 5 фотографий компаний
    private int _insideImagesId = 36; // смотреть в сиде в проекте Storage. Там доступно 50 фотографий компаний
    private const int InsideImagesCount = 50;

    private ICollection<string> FillList()
    {
        const int count = 10;
        var list = Enumerable.Range(_insideImagesId, count).ToList();
        _insideImagesId += count;
        return list.Select(c => $"{c}").ToList();
    }

    private Faker<Company> GetCompanyFaker()
    {
        var fake = new Faker<Company>()
                .RuleFor(u => u.Title, (f, _) => f.Company.CompanyName())
                .RuleFor(u => u.Description, (f, _) => f.Lorem.Paragraphs().TakeOnly(500))
                .RuleFor(u => u.Logo, (f, _) => $"{_logoId++}")
                .RuleFor(u => u.InsideImages, (f, _) => FillList())
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
            ;

        return fake;
    }

    private Faker<CompanyUser> GetCompanyUserFaker()
    {
        var fake = new Faker<CompanyUser>()
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
            ;

        return fake;
    }

    private const int TaskInfoImageId = 87;
    private const int TaskInfoImageCount = 6;

    private string? GetRandomImageOrNull()
    {
        var random = new Random();

        if (random.Next(0, 2) == 1)
        {
            return $"{random.Next(TaskInfoImageId, TaskInfoImageId + TaskInfoImageCount)}";
        }
        return null;
    }

    private Faker<TaskInfo> GetTaskInfoFaker()
    {
        var fake = new Faker<TaskInfo>()
                .RuleFor(u => u.Description, (f, _) => f.Lorem.Sentences(5).TakeOnly(500))
                .RuleFor(u => u.File, (_, _) => GetRandomImageOrNull())
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
            ;

        return fake;
    }

    private Faker<Project> GetProjectFaker()
    {
        var fake = new Faker<Project>()
                .RuleFor(u => u.Name, (f, _) => f.Name.JobArea())
                .RuleFor(u => u.Description, (f, _) => f.Lorem.Sentences().TakeOnly(500))
                .RuleFor(u => u.ProjectStatus, (f, _) => f.PickRandom<ProjectStatus>())
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
            ;

        return fake;
    }

    private Faker<ProjectTask> GetProjectTaskFaker()
    {
        var fake = new Faker<ProjectTask>()
                .RuleFor(u => u.Name, (f, _) => f.Name.JobArea())
                .RuleFor(u => u.Description, (f, _) => f.Lorem.Sentences().TakeOnly(500))
                .RuleFor(u => u.TaskStatus, (f, _) => f.PickRandom<ProjectTaskStatus>())
                .RuleFor(u => u.File, (_, _) => GetRandomImageOrNull())
                .RuleFor(u => u.Priority, (f, _) => f.PickRandom<TaskPriority>())
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
                .RuleFor(u => u.EstimatedTime, (f, _) => TimeSpan.FromHours(f.Random.Int(1, 8)))
                .RuleFor(u => u.TimeSpent, (f, _) => TimeSpan.FromHours(f.Random.Int(0, 8)))
            ;

        return fake;
    }

    private Faker<Team> GetTeamFaker()
    {
        var fake = new Faker<Team>()
                .RuleFor(u => u.Name, (f, _) => f.Name.JobArea())
                .RuleFor(u => u.Description, (f, _) => f.Lorem.Sentences().TakeOnly(500))
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
            ;

        return fake;
    }

    #endregion
}