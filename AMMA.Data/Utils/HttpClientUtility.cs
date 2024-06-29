namespace AMMA.Data.Utils;

public class HttpClientUtility
{
    private static HttpClientUtility? _instance;
    private readonly HttpClient _httpClient;

    private HttpClientUtility()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(Constants.ServiceUri);
    }

    public static HttpClientUtility Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new HttpClientUtility();
            }
            return _instance;
        }
    }

    public HttpClient GetHttpClient()
    {
        return _httpClient;
    }
}