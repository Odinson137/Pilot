using Bogus;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;
using Pilot.Receiver.Models;

namespace Pilot.Receiver.Data;

public class Seed : ISeed
{
    private readonly DataContext _context;

    private readonly List<CompanyRole> _companyRoles =
    [
        new CompanyRole
        {
            Title = "Owner",
            IsBaseRole = true
        },
        new CompanyRole
        {
            Title = "Developer",
            IsBaseRole = true
        },
        new CompanyRole
        {
            Title = "Designer",
            IsBaseRole = true
        }
    ];

    public Seed(DataContext context)
    {
        _context = context;
    }
    
    public async Task Seeding()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        
        // Добавляем базовые роли для всех компаниях
        await _context.CompanyRoles.AddRangeAsync(_companyRoles);
        await _context.SaveChangesAsync();
        
        var companyFaker = GetCompanyFaker();
        var companyRoleFaker = GetCompanyRoleFaker();
        var companyUserFaker = GetCompanyUserFaker();
        var projectFaker = GetProjectFaker();
        var projectTaskFaker = GetProjectTaskFaker();
        var teamFaker = GetTeamFaker();
        var taskInfoFaker = GetTaskInfoFaker();
        var rand = new Random();

        // Генерация компаний
        for (var i = 0; i < 5; i++)
        {
            var company = companyFaker.Generate();

            // Добавляем компанию в контекст сразу
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();  // Сохраняем, чтобы сгенерировать ключ
            
            // Генерация ролей компании
            company.CompanyRoles = companyRoleFaker.Generate(5);
            foreach (var role in company.CompanyRoles)
            {
                _context.CompanyRoles.Add(role);
            }

            await _context.SaveChangesAsync();  // Сохраняем роли

            var roles = await _context.CompanyRoles
                .Where(c => c.Companies.Any(x => x.Id == company.Id) || c.IsBaseRole == true)
                .ToListAsync();

            // Генерируем главного человека в компании
            var ownerCompany = new CompanyUser
            {
                Company = company,
                CompanyRole = roles.First(c => c.Title == "Owner")
            };

            await _context.CompanyUsers.AddAsync(ownerCompany);

            // Генерация пользователей
            var companyUsers = companyUserFaker.Generate(5); // 5 пользователей
            foreach (var user in companyUsers)
            {
                user.Company = company;

                user.CompanyRole = roles[rand.Next(0, roles.Count)];
                
                await _context.CompanyUsers.AddAsync(user);
            }

            await _context.SaveChangesAsync();  // Сохраняем пользователей

            // Генерация проектов
            var projects = projectFaker.Generate(rand.Next(3, 7)); // 3-7 проектов
            foreach (var project in projects)
            {
                project.Company = company;
                project.CreatedBy = ownerCompany;
                
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();  // Сохраняем проект, чтобы сгенерировать ключ
            }

            var users = await _context.CompanyUsers
                .Where(c => c.Company.Id == company.Id)
                .ToListAsync();
            
            // Генерация команд

            var projectsDb = await _context.Projects
                .Where(c => c.Company.Id == company.Id)
                .ToListAsync();
            
            var teams = teamFaker.Generate(rand.Next(2, 5)); // 2-5 команд
            foreach (var team in teams)
            {
                team.Project = projectsDb[rand.Next(0, projectsDb.Count)];

                for (var a = 0; a <= rand.Next(1, 3); a++)
                {
                    var user = users[rand.Next(1, 3)];
                    if (!team.CompanyUsers.Contains(user))
                    {
                        team.CompanyUsers.Add(user);
                    }
                }
                
                _context.Teams.Add(team);
                
                // Генерация задач проекта
                var projectTasks = projectTaskFaker.Generate(rand.Next(5, 15)); // 5-15 задач
                foreach (var task in projectTasks)
                {
                    // Получаем случайного пользователя
                    var randomUserIndex = rand.Next(-1, users.Count);
                    var randomCreated = users[rand.Next(0, users.Count)];

                    if (randomUserIndex != -1)
                    {
                        task.CompanyUser = users[randomUserIndex];
                    }
                    
                    task.CreatedBy = randomCreated;  // Назначаем пользователя задаче
                    task.Team = team;  // Назначаем команду задаче
    
                    _context.ProjectTasks.Add(task);
                }
                
                await _context.SaveChangesAsync();  // Сохраняем команды с задачами
            }

            var projectTasksDb = await _context.ProjectTasks
                .Where(c => c.Team.Project.Company.Id == company.Id)
                .ToListAsync();

            // Генерация дополнительных файлов для тасок
            foreach (var projectTask in projectTasksDb)
            {
                var taskInfos = taskInfoFaker.Generate(rand.Next(2, 3));

                foreach (var taskInfo in taskInfos)
                {
                    taskInfo.CreatedBy = ownerCompany;
                    taskInfo.ProjectTask = projectTask;
                }

                await _context.AddRangeAsync(taskInfos);
            }
            
            await _context.SaveChangesAsync();  // Сохраняем пользователей команд
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
    
    private Faker<Company> GetCompanyFaker()
    {
        var fake = new Faker<Company>()
                .RuleFor(u => u.Title, (f, _) => f.Company.CompanyName())
                .RuleFor(u => u.Description, (f, _) => f.Lorem.Paragraphs().TakeOnly(500))
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
    
    private Faker<HistoryAction> GetHistoryActionFaker()
    {
        var fake = new Faker<HistoryAction>()
                .RuleFor(u => u.ActionState, (f, _) => f.PickRandom<ActionState>())
                .RuleFor(u => u.LastValue, (f, _) => f.Lorem.Word())
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
            ;
        
        return fake;
    }
    
    private Faker<TaskInfo> GetTaskInfoFaker()
    {
        var fake = new Faker<TaskInfo>()
                .RuleFor(u => u.Description, (f, _) => f.Lorem.Sentences(5).TakeOnly(500))
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
                .RuleFor(u => u.CreateAt, (f, _) => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
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