using AMMA.Data;
using AMMA.Data.Utils;
using MAUI.Utils;

namespace MAUI.View;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        CheckLoginStateAsync();
    }
    
    private async void CheckLoginStateAsync()
    {
        var isLoggedIn = await AuthUtility.Instance.IsLoggedIn();
        if (isLoggedIn) { SetupMain(); }
    }
    
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var success = await AuthUtility.Instance.LoginAsync();
        if (success)
        {
            LoginBtn.IsVisible = false;
            SetupMain();
        }
        else
        {
            await DisplayAlert("Login Failed", "Unable to log in. Please try again.", "OK");
        }
    }

    private async void SetupMain()
    {
        LoaderView.IsVisible = true;
        await DatabaseUtility.Instance.InitializeDatabaseAsync(AMMA.Data.Constants.DatabasePath);
        var current = Application.Current;
        if (current is not null) {
            current.MainPage = new AppShell();
        }
    }
}