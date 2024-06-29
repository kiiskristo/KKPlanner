using MAUI.ViewModel;

namespace MAUI.View;

public partial class AssessmentDetailPage : ContentPage
{
    public AssessmentDetailPage(AssessmentDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        NameEntry.Unfocus();
    }
}