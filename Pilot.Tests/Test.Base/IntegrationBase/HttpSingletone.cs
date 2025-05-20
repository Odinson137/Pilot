namespace Test.Base.IntegrationBase;

public class HttpSingleTone
{
    private static HttpSingleTone? _httpSingleTone;

    public Dictionary<string, HttpClient> HttpClients = new();

    private HttpSingleTone()
    {
    }

    public static HttpSingleTone Init => _httpSingleTone ??= new HttpSingleTone();
    public static void Dispose() => _httpSingleTone?.HttpClients.Clear();
}