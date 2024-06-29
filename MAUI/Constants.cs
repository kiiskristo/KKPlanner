namespace MAUI;

public static class Constants
{
    private const string DatabaseFilename = "AcademicPlanner.db3";

    public const SQLite.SQLiteOpenFlags Flags = 
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath => 
        Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), DatabaseFilename);
}

public static class FaSolid
{
    public const string Plus = "\uf067";
    public const string Cross = "\uf00d";
    public const string Share = "\uf064";
    public const string Edit = "\uf142";
}