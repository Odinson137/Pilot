using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using YamlDotNet.RepresentationModel;

namespace NewBaseServiceGenerate;

internal static class Program
{
    private static string _projectName = null!;
    private const string Solution = "Pilot.";
    private const string Test = "Test.";
    private const string LevelUp = "../../../../../";
    private static string ProjectFullName => $"{Solution}{_projectName}";
    private static string TestProjectFullName => $"{Test}{_projectName}";
    private static string ProjectNameWithPath => $"{LevelUp}{ProjectFullName}";

    static void Main()
    {
        Console.WriteLine("Введите название проекта:");
        _projectName = Console.ReadLine()?.Trim() ?? "MyNewWebApi";

        // Создать проект Web API
        CreateWebApiProject();

        // Создать проект NUnit с тестами для Web API
        CreateTestForWebApiProject();

        // Добавить дополнительные папки
        CreateFolders();
        
        // Создать Dockerfile
        CreateDockerfile();
        
        // Добавление сервиса в compose
        AddServiceToCompose();

        // Добавление сервисы в гит
        AddFilesToGit();
        
        Console.WriteLine($"Проект {ProjectFullName} успешно создан!");
    }

    static void AddFilesToGit()
    {
        Console.WriteLine("Создаю в гит проекты...");
        // RunDotnetCommand("git add .", string.Empty);
    }

    static void CreateWebApiProject()
    {
        Console.WriteLine("Создаю проект Web API...");

        RunDotnetCommand("new webapi", $"-n {ProjectFullName} -o {ProjectNameWithPath}");

        Console.WriteLine($"Проект {ProjectFullName} успешно создан.");
        
        IncludeProjectToSolution(ProjectFullName, $"{ProjectFullName}\\{ProjectFullName}.csproj");
        
        Console.WriteLine("Добавляем ссылки на нужные проекты");
        RunDotnetCommand("add", $"{ProjectNameWithPath} reference {LevelUp}Pilot.Contracts/Pilot.BaseContract");
        RunDotnetCommand("add", $"{ProjectNameWithPath} reference {LevelUp}Pilot.Contracts/Pilot.SqrsControllerLibrary");

        Console.WriteLine("Добавляю необходимые библиотеки");
        AddPackagesToProject();
    }

    static void CreateFolders()
    {
        Console.WriteLine("Создаю дополнительные папки...");

        const string source = $"{LevelUp}Utils/NewBaseServiceGenerate/Templates";
        var folders = CopyTemplatesToProject(source, ProjectNameWithPath);
        AddFoldersToProjectFile($@"{ProjectNameWithPath}\{ProjectFullName}.csproj", folders);
    }
    
    static ICollection<string> CopyTemplatesToProject(string sourceDirectory, string targetDirectory)
    {
        if (!Directory.Exists(sourceDirectory))
        {
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDirectory}");
        }

        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        // Коллекция для хранения путей папок
        ICollection<string> folders = new List<string>();

        // Копируем все директории
        foreach (var directory in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(sourceDirectory, directory);
            var targetSubDirectory = Path.Combine(targetDirectory, relativePath);
        
            Directory.CreateDirectory(targetSubDirectory);
            folders.Add(relativePath.Replace(Path.DirectorySeparatorChar, '/'));
        }

        // Копируем и редактируем файлы
        foreach (var file in Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(sourceDirectory, file);
            var targetFile = Path.Combine(targetDirectory, relativePath);

            if (Path.GetExtension(file).Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                targetFile = Path.ChangeExtension(targetFile, ".cs");
            }

            // Чтение содержимого файла и замена шаблонных переменных
            var content = File.ReadAllText(file);
            content = content.Replace("%ProjectFullName%", ProjectFullName);

            File.WriteAllText(targetFile, content);
        }

        return folders;
    }

    static void CreateDockerfile()
    {
        Console.WriteLine("Создаю Dockerfile...");

        var dockerfilePath = Path.Combine(ProjectNameWithPath, "Dockerfile");
        var dockerfileContent = """

                                FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
                                USER $APP_UID
                                WORKDIR /app
                                EXPOSE 8080
                                EXPOSE 8081

                                FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
                                ARG BUILD_CONFIGURATION=Release
                                WORKDIR /src
                                COPY ["TestWebApplication/TestWebApplication.csproj", "TestWebApplication/"]
                                RUN dotnet restore "WebApplication1/WebApplication1.csproj"
                                COPY . .
                                WORKDIR "/src/TestWebApplication"
                                RUN dotnet build "TestWebApplication.csproj" -c $BUILD_CONFIGURATION -o /app/build

                                FROM build AS publish
                                ARG BUILD_CONFIGURATION=Release
                                RUN dotnet publish "TestWebApplication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

                                FROM base AS final
                                WORKDIR /app
                                COPY --from=publish /app/publish .
                                ENTRYPOINT ["dotnet", "TestWebApplication.dll"]


                                """.Replace("TestWebApplication", ProjectFullName);

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

        Console.WriteLine("Добавляю новый сервис в файл compose...");

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
            .SelectMany(entry =>
                ((YamlSequenceNode)entry.Value).Select(port => int.Parse(port.ToString().Split(':')[0])))
            .Max();

        var newPort = lastPort + 10;
        var name = _projectName.ToLower();
        var newServiceNode = new YamlMappingNode
        {
            {
                "build", new YamlMappingNode
                {
                    { "context", "." },
                    { "dockerfile", $"{ProjectFullName}/Dockerfile" }
                }
            },
            { "ports", new YamlSequenceNode($"{newPort}:8080") },
            { "depends_on", new YamlMappingNode
            {
                { $"pilot_{name}_mysql", "." },
                { "dockerfile", $"{ProjectFullName}/Dockerfile" }
            }}
        };

        var serviceName = name + "_service";
        servicesNode.Add(new YamlScalarNode(serviceName), newServiceNode);

        // не спрашивайте как я это считаю, но это должно работать)
        var newDbPort = lastPort - 4757 - 9;
        
        var newDbServiceNode = new YamlMappingNode
        {
            { "container_name", $"pilot-{name}-mysql" },
            { "image", "mysql:8.0" },
            { "ports", new YamlSequenceNode($"{newDbPort}:3306") },
            { "environment", new YamlMappingNode
                {
                    { "MYSQL_DATABASE", "PilotDb" },
                    { "MYSQL_ROOT_PASSWORD", "12345678" }
                }
            },
            { "healthcheck", new YamlMappingNode
                {
                    { "test", new YamlSequenceNode("CMD", "mysqladmin", "ping", "-h", "localhost") },
                    { "timeout", "10s" },
                    { "retries", "10" }
                }
            }
        };

        var serviceDbName = $"pilot_{name}_mysql";
        servicesNode.Add(new YamlScalarNode(serviceDbName), newDbServiceNode);
        using (var writer = new StreamWriter(filePath, false))
        {
            yaml.Save(writer, false);
        }

        Console.WriteLine($"Service '{ProjectFullName}' added with port {newPort}.");
    }

    static void CreateTestForWebApiProject()
    {
        var testProjectPath = $"{LevelUp}Pilot.Tests/{TestProjectFullName}";

        Console.WriteLine("Создаю проект для тестов...");
        RunDotnetCommand("new xunit", $"-n {TestProjectFullName} -o {testProjectPath}");

        Console.WriteLine("Добавляю ссылку на основной проект в тестах...");
        RunDotnetCommand("add", $"{testProjectPath} reference {ProjectNameWithPath}");

        RunDotnetCommand("add", $"{testProjectPath} reference {LevelUp}Pilot.Tests/Test.Base");

        Console.WriteLine("Проект с тестами успешно создан и настроен.");

        IncludeProjectToSolution(TestProjectFullName,
            $@"Pilot.Tests\{TestProjectFullName}\{TestProjectFullName}.csproj", isTest: true);
    }

    static void RunDotnetCommand(string command, string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"{command} {arguments}",
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                RedirectStandardError = true,
                RedirectStandardInput = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                ErrorDialog = false
            }
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var errorMessage = process.StandardError.ReadToEnd();
            Console.WriteLine($"Ошибка выполнения команды: {errorMessage}");
            throw new Exception(errorMessage);
        }

        Console.WriteLine(process.StandardOutput.ReadToEnd());
    }

    static void IncludeProjectToSolution(string projectName, string projectPath, bool isTest = false)
    {
        var solutionFilePath = $"{LevelUp}Pilot.sln";
        var projectGuid = Guid.NewGuid().ToString().ToUpper();
        var projectTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

        var projectEntry = $$"""
                             Project("{{projectTypeGuid}}") = "{{projectName}}", "{{projectPath}}", "{{{projectGuid}}}"
                             EndProject
                             """;

        var solutionLines = File.ReadAllLines(solutionFilePath).ToList();

        var insertIndex = solutionLines.FindLastIndex(line => line.StartsWith("EndProject")) + 1;
        if (insertIndex <= 0)
        {
            Console.WriteLine("Не удалось найти секцию Projects в решении.");
            return;
        }

        solutionLines.Insert(insertIndex, projectEntry);

        var globalIndex =
            solutionLines.FindIndex(line => line.Contains("GlobalSection(ProjectConfigurationPlatforms)"));
        if (globalIndex > 0)
        {
            var configurationEntry = $$"""
                                       		{{{projectGuid}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                       		{{{projectGuid}}}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                       		{{{projectGuid}}}.Release|Any CPU.ActiveCfg = Release|Any CPU
                                       		{{{projectGuid}}}.Release|Any CPU.Build.0 = Release|Any CPU
                                       """;

            solutionLines.Insert(globalIndex + 2, configurationEntry);
        }
        else
        {
            Console.WriteLine("Секция GlobalSection(ProjectConfigurationPlatforms) не найдена.");
        }

        if (isTest)
        {
            var nestedProjectsIndex = solutionLines.FindIndex(line => line.Contains("GlobalSection(NestedProjects)"));
            if (nestedProjectsIndex > 0)
            {
                var nestedProjectEntry = $$"""
                                                   {{{projectGuid}}} = {D9F9AFD5-CE30-4950-8B16-A285EBEF5008}
                                           """; // guid папки с тестовыми проектами
                solutionLines.Insert(nestedProjectsIndex + 1, nestedProjectEntry);
            }
        }

        File.WriteAllLines(solutionFilePath, solutionLines);

        Console.WriteLine("Проект успешно добавлен в решение.");
    }
    
    static void AddFoldersToProjectFile(string projectFilePath, ICollection<string> folders)
    {
        if (!File.Exists(projectFilePath))
        {
            Console.WriteLine("Файл проекта не найден: " + projectFilePath);
            return;
        }

        var projectDirectory = Path.GetDirectoryName(projectFilePath);
        if (string.IsNullOrEmpty(projectDirectory))
        {
            Console.WriteLine("Не удалось определить директорию проекта.");
            return;
        }

        var projectXml = XDocument.Load(projectFilePath);

        var itemGroup = projectXml.Root?.Elements("ItemGroup")
            .FirstOrDefault(group => group.Elements("Folder").Any());

        if (itemGroup == null)
        {
            itemGroup = new XElement("ItemGroup");
            projectXml.Root?.Add(itemGroup);
        }

        foreach (var folder in folders)
        {
            itemGroup.Add(new XElement("Folder", new XAttribute("Include", folder)));
        }

        projectXml.Save(projectFilePath);
        Console.WriteLine("Папки успешно добавлены в файл проекта.");
    }

    static void AddPackagesToProject()
    {
        var projectFile = Directory.GetFiles($"{ProjectNameWithPath}", "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
        if (projectFile == null)
        {
            throw new FileNotFoundException("Project file (.csproj) not found in the target directory.");
        }

        var lines = File.ReadAllLines(projectFile).ToList();

        var itemGroupIndex = lines.FindIndex(line => line.Contains("<ItemGroup>"));
        if (itemGroupIndex == -1)
        {
            throw new InvalidOperationException("<ItemGroup> section not found in the project file.");
        }

        lines.InsertRange(itemGroupIndex + 1, [
            "    <PackageReference Include=\"Microsoft.AspNetCore.OpenApi\" Version=\"8.0.11\" />",
            "    <PackageReference Include=\"Swashbuckle.AspNetCore\" Version=\"6.6.2\" />"
        ]);

        File.WriteAllLines(projectFile, lines);
    }
}