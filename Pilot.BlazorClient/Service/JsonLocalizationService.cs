using System.Text.Json;

namespace Pilot.BlazorClient.Service;

public interface IJsonLocalizationService
{
    string GetString(string key, string pageName);

    void Toggle();
    
    string? CurrentCulture { get; }
}

public class JsonLocalizationService : IJsonLocalizationService
{
    private readonly IWebHostEnvironment _env;
    private readonly Dictionary<string, Dictionary<string, string>?> _localizationData = new();

    public JsonLocalizationService(IWebHostEnvironment env)
    {
        _env = env;
    }

    private bool _isChange;
    
    public void Toggle()
    {
        _isChange = true;
        CurrentCulture = CurrentCulture == "en-US" ? "ru-RU" : "en-US";
    }

    public string CurrentCulture { get; private set; } = "en-US";

    public string GetString(string key, string pageName)
    {
        if (!_localizationData.ContainsKey(pageName))
            LoadLocalizationData(pageName, CurrentCulture);

        if (_isChange)
        {
            var names = _localizationData.Select(c => c.Key).ToList();
            
            foreach (var name in names)
            {
                LoadLocalizationData(name, CurrentCulture);
            }
            
            _isChange = false;
        }

        return _localizationData[pageName]!.TryGetValue(key, out var value) ? value : key;
    }

    private void LoadLocalizationData(string pageName, string culture)
    {
        var filePath = Path.Combine(_env.ContentRootPath, "wwwroot/Resources", culture, $"{pageName}.json");
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _localizationData[pageName] = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        else
        {
            _localizationData[pageName] = new Dictionary<string, string>();
        }
    }
}