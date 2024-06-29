using AMMA.Data.ViewModel;
namespace MAUI.View;

[QueryProperty(nameof(TermId), "termId")]
public partial class TermDetailPage : ContentPage
{
    public string TermId
    {
        set => ((TermDetailViewModel)BindingContext).ApplyQueryAttributes(new Dictionary<string, object> { { "termId", value } });
    }
    
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