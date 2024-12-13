using System.Diagnostics;
using YamlDotNet.RepresentationModel;

namespace NewBaseServiceGenerate;

class Program
{
    private static string _projectName = null!;
    private const string LevelUp = "../../../../../";
    private static string ProjectNameWithPath => $"{LevelUp}{_projectName}";
    
    static void Main()
    {
        Console.WriteLine("Введите название проекта:");
        _projectName = Console.ReadLine()?.Trim() ?? "MyNewWebApi";

        // Создать проект Web API
        CreateWebApiProject();

        // Добавить дополнительные папки
        CreateFolders();

        // Создать Dockerfile
        CreateDockerfile();
        
        // Добавление сервиса в compose
        AddServiceToCompose();

        Console.WriteLine($"Проект {_projectName} успешно создан!");
        // лучшего решения не придумал. Или придется парится с добавлением проекта в workspace райдера
        Console.WriteLine("Для отображения проекта в списке проектов тыкните на solution и добавьте в него существующий проект вручную");
    }

    static void CreateWebApiProject()
    {
        Console.WriteLine("Создаю проект Web API...");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"new webapi -n {_projectName} -o {ProjectNameWithPath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode == 0)
            Console.WriteLine($"Проект {_projectName} успешно создан.");
        else
        {
            Console.WriteLine($"Ошибка при создании проекта: {process.StandardError.ReadToEnd()}");
            throw new Exception(process.StandardError.ReadToEnd());
        }
    }

    static void CreateFolders()
    {
        Console.WriteLine("Создаю дополнительные папки...");

        string[] folders = { "Consumers", "Controllers", "Data", "Models", "Interface", "Repository", "Service" };
        foreach (var folder in folders)
        {
            var folderPath = Path.Combine(ProjectNameWithPath, folder);
            Directory.CreateDirectory(folderPath);
            Console.WriteLine($"Папка {folder} создана.");
        }
    }

    static void CreateDockerfile()
    {
        Console.WriteLine("Создаю Dockerfile...");

        var dockerfilePath = Path.Combine(ProjectNameWithPath, "Dockerfile");
        var dockerfileContent = @"
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY [""TestWebApplication/TestWebApplication.csproj"", ""TestWebApplication/""]
RUN dotnet restore ""WebApplication1/WebApplication1.csproj""
COPY . .
WORKDIR ""/src/TestWebApplication""
RUN dotnet build ""TestWebApplication.csproj"" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ""TestWebApplication.csproj"" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [""dotnet"", ""TestWebApplication.dll""]

".Replace("TestWebApplication", _projectName);

        File.WriteAllText(dockerfilePath, dockerfileContent);
        Console.WriteLine("Dockerfile создан.");
    }
    
    static void AddServiceToCompose()
    {
        var filePath = Path.Combine(LevelUp, "compose.yaml");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл compose.yaml не найден: {filePath}");
        }

        Console.WriteLine($"Добавляю новый сервис в файл compose...");
        
        var yaml = new YamlStream();
        using (var reader = new StreamReader(filePath))
        {
            yaml.Load(reader);
        }
        var rootNode = (YamlMappingNode)yaml.Documents[0].RootNode;
        var servicesNode = (YamlMappingNode)rootNode.Children[new YamlScalarNode("services")];

        var lastPort = servicesNode.Children
            .Where(child => child.Value is YamlMappingNode)
            .Where(child => child.Key.ToString().Contains("_server"))
            .SelectMany(child => ((YamlMappingNode)child.Value).Children)
            .Where(entry => entry.Key.ToString() == "ports")
            .SelectMany(entry => ((YamlSequenceNode)entry.Value).Select(port => int.Parse(port.ToString().Split(':')[0])))
            .Max();

        var newPort = lastPort + 10;
        var newServiceNode = new YamlMappingNode
        {
            { "build", new YamlMappingNode
                {
                    { "context", "." },
                    { "dockerfile", $"{_projectName}/Dockerfile" }
                }
            },
            { "ports", new YamlSequenceNode($"{newPort}:8080") }
        };

        servicesNode.Add(new YamlScalarNode(_projectName), newServiceNode);

        using (var writer = new StreamWriter(filePath))
        {
            yaml.Save(writer);
        }

        Console.WriteLine($"Service '{_projectName}' added with port {newPort}.");
    }

}