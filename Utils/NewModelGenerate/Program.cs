namespace NewModelGenerate;

internal static class Program
{
    private const string LevelUp = "../../../../";
    private static string? _modelName = null!;

    public static void Main(string[] args)
    {
        var projects = Directory.GetDirectories(LevelUp)
            .Where(dir => dir.Contains("Pilot.") && !dir.EndsWith("s"))
            .Select(dir => new { Name = Path.GetFileName(dir), Path = dir })
            .ToList();

        if (projects.Count == 0)
        {
            Console.WriteLine("В указанной директории не найдено проектов.");
            return;
        }

        Console.WriteLine("Найдены проекты:");
        for (var i = 0; i < projects.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {projects[i].Name}");
        }

        Console.WriteLine("Введите номер проекта, с которым хотите работать:");
        if (!int.TryParse(Console.ReadLine(), out var projectIndex) || projectIndex < 1 ||
            projectIndex > projects.Count)
        {
            Console.WriteLine("Некорректный выбор проекта.");
            return;
        }

        var selectedProject = projects[projectIndex - 1];
        Console.WriteLine($"Выбран проект: {selectedProject.Name}");

        Console.WriteLine("Введите название будущей модели:");
        _modelName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(_modelName))
        {
            Console.WriteLine("Название модели не может быть пустым.");
            return;
        }

        const string currentProject = $"{LevelUp}NewModelGenerate";
        ProcessTemplate($"{currentProject}/Service/Model.txt", $"{selectedProject.Path}/Models", selectedProject.Name);
        ProcessTemplate($"{currentProject}/Base/ModelDto.txt",
            $"{LevelUp}Pilot.Contracts/Pilot.BaseContract/Dto/ModelDto", selectedProject.Name, "Dto");
        ProcessTemplate($"{currentProject}/Service/Controller.txt", $"{selectedProject.Path}/Controllers",
            selectedProject.Name, "Controller");
        ProcessTemplate($"{currentProject}/Api/Controller.txt", $"{LevelUp}Pilot.Api/Controller", selectedProject.Name,
            "Controller");
        ProcessTemplate($"{currentProject}/Api/QueryHandler.txt", $"{LevelUp}Pilot.Api/Handlers", selectedProject.Name,
            "QueryHandler");
        ProcessTemplate($"{currentProject}/Api/CommandHandler.txt", $"{LevelUp}Pilot.Api/Handlers",
            selectedProject.Name, "CommandHandler");
        ProcessTemplate($"{currentProject}/Service/Handler.txt", $"{selectedProject.Path}/Handlers",
            selectedProject.Name, "CommandHandler");
        ProcessTemplate($"{currentProject}/Service/IRepository.txt", $"{selectedProject.Path}/Interface",
            selectedProject.Name, beforeName: "I");
        ProcessTemplate($"{currentProject}/Service/Repository.txt", $"{selectedProject.Path}/Repository",
            selectedProject.Name, "Repository");

        // TODO сделать потом добавление в маппинги и AddScoped
        
        Console.WriteLine("Файлы успешно добавлены!");
    }

    static void ProcessTemplate(string templatePath, string targetPath, string projectName, string? afterName = null,
        string? beforeName = null)
    {
        var templateContent = File.ReadAllText(templatePath);

        templateContent = templateContent.Replace("%ProjectFullName%", projectName);
        if (projectName.Contains('.'))
        {
            templateContent = templateContent.Replace("%ProjectName%", projectName.Split('.')[1]);
        }

        templateContent = templateContent.Replace("%ModelName%", _modelName);

        File.WriteAllText($"{targetPath}/{beforeName}{_modelName}{afterName}.cs", templateContent);
    }
}