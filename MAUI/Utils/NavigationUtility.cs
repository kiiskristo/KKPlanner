using AMMA.Data.Utils;

namespace MAUI.Utils;

public class NavigationUtility : INavigationUtility
{
    public void NavigateTo(string newRoute, bool appendToRoute = false)
    {
        if (Application.Current?.Dispatcher is not null)
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                var currentRoute = Shell.Current.CurrentState.Location.ToString();
                if (appendToRoute)
                {
                    newRoute = $"{currentRoute}/{newRoute}";
                }
                Console.WriteLine($"let's go to places from: {currentRoute} to: {newRoute}");
                Shell.Current.GoToAsync(newRoute);
            });
        }
    }
    
    public void OpenModalTo(string route)
    {
        if (Application.Current?.Dispatcher is not null)
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                Console.WriteLine($"let's go to places {route}");
                Shell.Current.GoToAsync(route, true);
            });
        }
    }

    public async Task<string>  ActionSheet(string title, List<string> options)
    {
        var actionSheetOptions = new List<string>(options);
        return await Shell.Current.DisplayActionSheet(title, "Cancel", null, actionSheetOptions.ToArray());
    }

    public void NavigateBack(object? parameter = null)
    {
        Console.WriteLine($"let's go back");
        Shell.Current.GoToAsync("..");
    }

    public void Share(string notes)
    {
        /*
        await Share.RequestAsync(new ShareTextRequest
        {
            Text = notes,
            Title = "Share Notes"
        });
        */
    }
    
}