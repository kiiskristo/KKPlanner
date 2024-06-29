using AMMA.Data.ViewModel;

namespace MAUI.View;

[QueryProperty(nameof(CourseId), "courseId")]
[QueryProperty(nameof(AssessmentId), "assessmentId")]
public partial class AssessmentDetailPage : ContentPage
{
    public string CourseId
    {
        set => ((AssessmentDetailViewModel)BindingContext).ApplyQueryAttributes(new Dictionary<string, object> { { "courseId", value } });
    }
    public string AssessmentId
    {
        set => ((AssessmentDetailViewModel)BindingContext).ApplyQueryAttributes(new Dictionary<string, object> { { "assessmentId", value } });
    }
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