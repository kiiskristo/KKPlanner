using MAUI.Model;
using MAUI.ViewModel;

namespace MAUI.View;

public partial class TermDetailPage : ContentPage
{
    public TermDetailPage(TermDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((TermDetailViewModel)BindingContext).RefreshDetails();
    }
}