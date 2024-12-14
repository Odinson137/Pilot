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
        if (!int.TryParse(Console.ReadLine(), out var projectIndex) || projectIndex < 1 || projectIndex > projects.Count)
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

        ProcessTemplate($"{LevelUp}NewModelGenerate/Service/Model.txt", $"{selectedProject.Path}/Models", selectedProject.Name);
        Console.WriteLine("Файлы успешно обработана и сохранены по указанному пути.");

    }
    
    static void ProcessTemplate(string templatePath, string targetPath, string projectName)
    {
        var templateContent = File.ReadAllText(templatePath);

        templateContent = templateContent.Replace("%ProjectFullName%", projectName);
        templateContent = templateContent.Replace("%ModelName%", _modelName);

        File.WriteAllText($"{targetPath}/{_modelName}.cs", templateContent);
    }
}