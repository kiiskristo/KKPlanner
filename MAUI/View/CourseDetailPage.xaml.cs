using AMMA.Data.ViewModel;
namespace MAUI.View;

[QueryProperty(nameof(TermId), "termId")]
[QueryProperty(nameof(CourseId), "courseId")]
public partial class CourseDetailPage : ContentPage
{
    public string TermId
    {
        set => ((CourseDetailViewModel)BindingContext).ApplyQueryAttributes(new Dictionary<string, object> { { "termId", value } });
    }
    public string CourseId
    {
        set => ((CourseDetailViewModel)BindingContext).ApplyQueryAttributes(new Dictionary<string, object> { { "courseId", value } });
    }
    
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