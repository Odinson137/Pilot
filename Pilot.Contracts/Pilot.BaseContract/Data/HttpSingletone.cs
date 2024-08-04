namespace Pilot.Contracts.Data;

public class HttpSingleTone
{
    private HttpSingleTone() { }

    public Dictionary<string, HttpClient> HttpClients = new();
    
    private static HttpSingleTone? _httpSingleTone;
    public static HttpSingleTone Init => _httpSingleTone ??= new HttpSingleTone();
}