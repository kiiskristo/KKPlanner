using AMMA.Data.ViewModel;

namespace MAUI.View;

public partial class ReportsPage : ContentPage
{
    public ReportsPage(ReportsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((ReportsViewModel)BindingContext).LoadReportsAsync();
    }
}