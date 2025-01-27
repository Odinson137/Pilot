﻿using System.Text.Json;

public interface IJsonLocalizationService
{
    string GetString(string key, string pageName);

    void ChangeCulture(string culture);
    void Toggle();
    
    string? CurrentCulture { get; }
}

public class JsonLocalizationService : IJsonLocalizationService
{
    private readonly IWebHostEnvironment _env;
    private readonly Dictionary<string, Dictionary<string, string>?> _localizationData = new();

    private string _currentCulture = "en-US";
    public JsonLocalizationService(IWebHostEnvironment env)
    {
        _env = env;
    }

    private bool _isChange;
    
    public void ChangeCulture(string culture)
    {
        _isChange = true;
        _currentCulture = culture;
    }

    public void Toggle()
    {
        _isChange = true;
        _currentCulture = _currentCulture == "en-US" ? "ru-RU" : "en-US";
    }

    public string? CurrentCulture => _currentCulture;

    public string GetString(string key, string pageName)
    {
        if (!_localizationData.ContainsKey(pageName) || _isChange)
        {
            var names = _localizationData.Select(c => c.Key).ToList();
            if (names.Count > 1)
            {
                foreach (var name in names)
                {
                    LoadLocalizationData(name, _currentCulture);
                }
            }
            else
            {
                LoadLocalizationData(pageName, _currentCulture);
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