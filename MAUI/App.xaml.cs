using MAUI.Services;
using MAUI.Utils;
using MAUI.View;
using Plugin.LocalNotification;

namespace MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new LoaderPage();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await DatabaseUtility.Instance.InitializeDatabaseAsync();
        MainPage = new AppShell();
    }
}