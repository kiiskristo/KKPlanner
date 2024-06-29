using MAUI.Utils;
using AMMA.Data.ViewModel;

namespace MAUI.View;

public partial class TermsPage : ContentPage
{
    public TermsPage(TermsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        var success = await AuthUtility.Instance.LogoutAsync();
        if (!success) { return; }
        var current = Application.Current;
        if (current is not null) {
            current.MainPage = new LoginPage();
        }
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((TermsViewModel)BindingContext).LoadTermsAsync();
    }
}
