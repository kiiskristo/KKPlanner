using Auth0.OidcClient;
using AMMA.Data;

namespace MAUI.Utils;

public class AuthUtility
{
    private static AuthUtility? _instance;
    private readonly Auth0Client _auth0Client;
    private string? _accessToken;
    private string? _refreshToken;
    private string? _userName;
    private string? _userPicture;

    public static AuthUtility Instance => _instance ??= new AuthUtility();

    private AuthUtility()
    {
        var options = new Auth0ClientOptions
        {
            Domain = Constants.Auth0_Domain,
            ClientId = Constants.Auth0_ClientId,
            RedirectUri = Constants.Auth0_RedirectUri,
            PostLogoutRedirectUri = Constants.Auth0_PostLogoutRedirectUri
        };

        _auth0Client = new Auth0Client(options);
    }

    public async Task<bool> LoginAsync()
    {
        var loginResult = await _auth0Client.LoginAsync();
        if (loginResult.IsError) { return false; }
        {
            _accessToken = loginResult.AccessToken;
            _refreshToken = loginResult.RefreshToken;
            _userName = loginResult?.User?.Identity?.Name;
            _userPicture = loginResult?.User?.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            await SecureStorage.SetAsync("IsLoggedIn", "true");
            return true;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        var logoutResult = await _auth0Client.LogoutAsync();
        SecureStorage.Remove("IsLoggedIn");
        return true;
    }

    public async Task<bool> RefreshTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(_refreshToken))
        {
            return false;
        }

        var refreshResult = await _auth0Client.RefreshTokenAsync(_refreshToken);
        if (!refreshResult.IsError)
        {
            _accessToken = refreshResult.AccessToken;
            _refreshToken = refreshResult.RefreshToken;
            return true;
        }

        return false;
    }

    public async Task<bool> IsLoggedIn()
    {
        var isLoggedIn = await SecureStorage.GetAsync("IsLoggedIn");
        return isLoggedIn == "true";
    }

    public string? GetAccessToken() => _accessToken;

    public string? UserName() => _userName;
    
    public string? UserPicture() => _userPicture;
}