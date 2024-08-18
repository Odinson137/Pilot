namespace Pilot.Contracts.Data;

public class HttpSingleTone
{
    private static HttpSingleTone? _httpSingleTone;

    public Dictionary<string, HttpClient> HttpClients = new();

    private HttpSingleTone()
    {
    }

    public static HttpSingleTone Init => _httpSingleTone ??= new HttpSingleTone();
}