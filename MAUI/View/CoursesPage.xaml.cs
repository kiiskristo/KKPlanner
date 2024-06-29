using MAUI.Model;
using MAUI.ViewModel;

namespace MAUI.View;

public partial class CoursesPage : ContentPage
{
    public CoursesPage(CoursesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((CoursesViewModel)BindingContext).LoadCourses();
    }
    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Course selectedCourse)
        {
            var viewModel = BindingContext as CoursesViewModel;
            viewModel?.EditCommand.Execute(selectedCourse);
        }
        if (sender is ListView listView)
        {
            listView.SelectedItem = null;
        }
    }
}