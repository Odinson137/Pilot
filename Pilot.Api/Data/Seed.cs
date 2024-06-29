using MongoDB.Driver;
using Pilot.Api.Data.Enums;
using Pilot.Contracts.Data;
using Pilot.Contracts.Models;

namespace Pilot.Api.Data;


public class Seed : ISeed
{
    public async Task Seeding(IApplicationBuilder app)
    {
        // var mongoDatabase = app.ApplicationServices.GetRequiredService<IMongoDatabase>();
        //
        // var companyCollection = mongoDatabase.GetCollection<Company>(MongoTable.Company);
        //
        // var companyExist = await companyCollection.CountDocumentsAsync(FilterDefinition<Company>.Empty);
        //
        // if (companyExist != 0)
        // {
        //     return;
        // }
        //
        // var companyUsers = new List<CompanyUser>()
        // {
        //     new ()
        //     {
        //         UserName = "Admin",
        //         Name = "Yuri",
        //         LastName = "Bury",
        //     },
        //     new ()
        //     {
        //         UserName = "Baget",
        //         Name = "Sasha",
        //         LastName = "Baginskiy",
        //     },
        //     new ()
        //     {
        //         UserName = "JSCooler",
        //         Name = "Yarick",
        //         LastName = "Yanovich",
        //     }
        // };
        //
        // var projectTasks = new List<ProjectTask>()
        // {
        //     new ProjectTask
        //     {
        //         Name = "Сделать дизайн в Jira",
        //         Description = "Продумать все страницы и создать макеты",
        //     },
        //     new ProjectTask
        //     {
        //         Name = "Создать начальные модели для бд",
        //         Description =
        //             "Создать хотя бы самые важные модели, чтоб потом на их основе можно было создавать тесты",
        //     },
        //     new ProjectTask
        //     {
        //         Name = "Сделать тесты для моделей программы",
        //         Description =
        //             "Протестировать соотносятся ли модели к ТЗ",
        //     },
        // };
        //
        // var teams = new List<Team>()
        // {
        //     new Team
        //     {
        //         Name = "Тестеры",
        //         Description = "Занимаются созданием тестов и их выполнением",
        //         ProjectTask = projectTasks[2],
        //         CompanyUsers = new List<CompanyUser>() { companyUsers[0] },
        //     },
        //     new Team
        //     {
        //         Name = "Модельщики",
        //         Description = "Занимаются созданием моделей и работой с бд",
        //         ProjectTask = projectTasks[1],
        //         CompanyUsers = new List<CompanyUser>() { companyUsers[1] },
        //     },
        //     new Team
        //     {
        //         Name = "Верстальщики",
        //         Description = "Занимаются дизайна и вёрстки проекта",
        //         ProjectTask = projectTasks[0],
        //         CompanyUsers = new List<CompanyUser>() { companyUsers[2] },
        //     }
        // };
        //
        // var projects = new List<Project>()
        // {
        //     new()
        //     {
        //         Name = "Author verse",
        //         Description = "The biggest and cooler company in the world",
        //         Teams = teams,
        //         ProjectTasks = projectTasks,
        //         ProjectStatus = ProjectStatus.Development,
        //     },
        //     new()
        //     {
        //         Name = "Forward",
        //         Description = "Just coll project",
        //         Teams = new List<Team>()
        //         {
        //             new Team
        //             {
        //                 Name = "Ни шагу назад",
        //                 Description = "Обучаться, обучатсья и обучаться",
        //                 ProjectTask = new ProjectTask
        //                 {
        //                     Name = "Сделать",
        //                     Description = "Просто сделать",
        //                 },
        //                 CompanyUsers = new List<CompanyUser>()
        //                 {
        //                     companyUsers[1],
        //                 }
        //             }
        //         },
        //         ProjectTasks = new List<ProjectTask>()
        //         {
        //               new ProjectTask
        //               {
        //                   Name = "Сделать на диплом",
        //                   Description = "Усовершенствовать данный проект, чтоб его можно было показать смело на диплом",
        //               }
        //         },
        //         ProjectStatus = ProjectStatus.Development,
        //     },
        //     new()
        //     {
        //         Name = "Расписание МРК",
        //         Description = "Program for helping the schedule to the college",
        //         Teams = new List<Team>()
        //         {
        //             new Team
        //             {
        //                 Name = "МРКашники",
        //                 Description = "Работа за халтуру",
        //                 ProjectTask = new ProjectTask
        //                 {
        //                     Name = "Сделать, чтоб было",
        //                     Description = "Просто завершить весь проект",
        //                 },
        //                 CompanyUsers = new List<CompanyUser>() { companyUsers[0] },
        //             }
        //         },
        //         ProjectStatus = ProjectStatus.Ready,
        //     },
        // };
        //
        // var companies = new List<Company>()
        // {
        //     new ()
        //     {
        //         Title = "Apple",
        //         Description = "Small company for creating new types computers",
        //     },
        //     new ()
        //     {
        //         Title = "Microsoft",
        //         Description = "My favorite corporation",
        //         CompanyUsers = companyUsers,
        //         Projects = projects,
        //     },
        //     new ()
        //     {
        //         Title = "Amazon",
        //         Description = "Target company",
        //     }
        // };
        //
        // var mongoCompanyCollection = mongoDatabase.GetCollection<Company>(MongoTable.Company);
        //
        // await mongoCompanyCollection.InsertManyAsync(companies);
    }
}