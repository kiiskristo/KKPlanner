using AMMA.Data.ViewModel;

namespace MAUI.View;

[QueryProperty(nameof(InstructorId), "instructorId")]
public partial class InstructorDetailPage : ContentPage
{
    public string InstructorId
    {
        set => ((InstructorDetailViewModel)BindingContext).ApplyQueryAttributes(new Dictionary<string, object> { { "instructorId", value } });
    }
    public InstructorDetailPage(InstructorDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}