using MAUI.ViewModel;

namespace MAUI.View;

public partial class CourseDetailPage : ContentPage
{
    public CourseDetailPage(CourseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((CourseDetailViewModel)BindingContext).RefreshDetails();
        StatusPicker.IsEnabled = false;
        TermPicker.IsEnabled = false;
        NameEntry.IsEnabled = false;
        EmailEntry.IsEnabled = false;
        PhoneEntry.IsEnabled = false;
        NotesEditor.IsEnabled = false;
        FocusTopFieldWithUxConsideration();
    }
    
    private async void FocusTopFieldWithUxConsideration()
    {
        await Task.Delay(500);
        StatusPicker.IsEnabled = true;
        StatusPicker.Unfocus();
        TermPicker.IsEnabled = true;
        NameEntry.IsEnabled = true;
        EmailEntry.IsEnabled = true;
        PhoneEntry.IsEnabled = true;
        NotesEditor.IsEnabled = true;
    }
}