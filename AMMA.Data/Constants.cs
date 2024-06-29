namespace AMMA.Data;

public static class Constants
{
    private const string DatabaseFilename = "AcademicPlanner.db3";

    public const SQLite.SQLiteOpenFlags Flags = 
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath => 
        Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), DatabaseFilename);
    
    public static string ServiceUri = "https://kkplanner.azurewebsites.net/";
    public static string SyncfusionKey = "Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxdcXVTQ2NZWEF+XkY=";
    
    public static string Auth0_Domain = "dev-cm6oa08qcum25xqg.us.auth0.com";
    public static string Auth0_ClientId = "JPX3yD92JjWM1b1IAsXMWDVRTVHEbXKk";
    public static string Auth0_RedirectUri = "myapp://callback/";
    public static string Auth0_PostLogoutRedirectUri = "myapp://callback/";
    
}

public static class FaSolid
{
    public const string Plus = "\uf067";
    public const string Share = "\uf064";
    public const string Edit = "\uf142";
}