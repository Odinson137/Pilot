namespace Pilot.BlazorClient.Data;

// хочу быстрее доделать проект и его дорабатывать
public static class TempFileService
{
    public static string GetUrl(string? fileName)
    {
        return fileName != null ? fileName.Contains("/Images/") ? fileName : $"https://storage.cloud.google.com/test_yuri_buryy/image/{fileName}" : "/Images/Account/default-avatar.jpg";
    }
}