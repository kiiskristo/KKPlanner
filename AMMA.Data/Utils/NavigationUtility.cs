namespace AMMA.Data.Utils;

public interface INavigationUtility
{
    Task<string> ActionSheet(string title, List<string> options);
    void NavigateTo(string newRoute, bool appendToRoute = false);
    void OpenModalTo(string route);
    void NavigateBack(object? parameter = null);
    void Share(string notes);
}